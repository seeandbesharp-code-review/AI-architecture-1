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
        
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Ok("value");
        }
        
        // POST api/<UsersController>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserDTO value, string password)
        {
            AuthResponseDTO? response = await _iUsersServicies.AddNewUser(value, password);
            if (response == null)
                return BadRequest("Password is too weak");
            AppendJwtCookie(response.Token);
            return CreatedAtAction(nameof(Get), new { response.User.id }, response.User);
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
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userToUpdate, string password)
        {
            bool passwordsStrenght = await _iUsersServicies.UpdateUser(id, userToUpdate, password);
            if (passwordsStrenght)
            {
                return Ok(userToUpdate);
            }
            return NoContent();        
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
