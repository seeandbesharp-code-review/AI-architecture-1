using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Services;
using DTO_s;

namespace Enteties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        IUsersService _iUsersServicies;
        IpasswordServices _iPasswordsServices;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(IUsersService usersServicies, IpasswordServices passwordServices, ILogger<UsersController> logger)
        {
            _iPasswordsServices = passwordServices;
            _iUsersServicies = usersServicies;
            _logger = logger;
        }

        private void AppendJwtCookie(string token)
        {
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
        }
        
        // POST api/<UsersController>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Post([FromBody] PostUserDTO value)
        {
            AuthResponseDTO? response = await _iUsersServicies.AddNewUser(value);
            if (response == null)
                return BadRequest("Password is too weak");
            AppendJwtCookie(response.Token);
            return CreatedAtAction(nameof(GetById), new { response.User.id }, response.User);
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> login([FromBody] ExisitingUser value)
        {
            AuthResponseDTO? response = await _iUsersServicies.Login(value);
            if (response != null)
            {
                _logger.LogInformation("Login attempted with User Email: " + value.Email);
                AppendJwtCookie(response.Token);
                return Ok(response.User);
            }
            _logger.LogError("Error in getting user");
            return Unauthorized();
        }
        
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] PostUserDTO userToUpdate)
        {
            bool success = await _iUsersServicies.UpdateUser(id, userToUpdate);
            if (!success)
                return BadRequest("Password is too weak");
            UserDTO updatedUser = await _iUsersServicies.GetById(id);
            return Ok(updatedUser);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetById(int id)
        {
            UserDTO user = await _iUsersServicies.GetById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

    }
}
