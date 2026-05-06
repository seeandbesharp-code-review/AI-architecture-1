using AutoMapper;
using DTO_s;
using Enteties;
using Repositories;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _iUsersRepository;
        private readonly IpasswordServices _passwordServices;
        private readonly IJwtService _jwtService;
        IMapper _mapper;

        public UsersService(IUsersRepository usersRepository, IpasswordServices passwordServices, IMapper imapper, IJwtService jwtService)
        {
            _iUsersRepository = usersRepository;
            _passwordServices = passwordServices;
            _mapper = imapper;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDTO?> AddNewUser(UserDTO userDTO, string password)
        {
            if (_passwordServices.GetStrength(password).Strength <= 2)
                return null;
            User user = _mapper.Map<UserDTO, User>(userDTO);
            user.Password = password;
            user.Role = "User"; // default role on registration
            User userResult = await _iUsersRepository.AddUser(user);
            UserDTO userDTOres = _mapper.Map<User, UserDTO>(userResult);
            string token = _jwtService.GenerateToken(userResult.Id, userResult.Email, userResult.FirstName, userResult.LastName, userResult.Role);
            return new AuthResponseDTO(userDTOres, token);
        }

        public async Task<AuthResponseDTO?> Login(ExisitingUser user)
        {
            User? result = await _iUsersRepository.login(user.Email, user.Password);
            if (result == null) return null;
            string token = _jwtService.GenerateToken(result.Id, result.Email, result.FirstName, result.LastName, result.Role);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(result);
            return new AuthResponseDTO(userDTO, token);
        }

        public async Task<bool> UpdateUser(int id, UserDTO userToUpdate, string password)
        {
            if (_passwordServices.GetStrength(password).Strength <= 2)
            {
                return false;
            }
            User user = _mapper.Map<UserDTO, User>(userToUpdate);
            user.Id = id;
            user.Password = password;
            await _iUsersRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<UserDTO> GetById(int id)
        {
            User user = await _iUsersRepository.GetById(id);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);
            return userDTO;
        }
    }
}
