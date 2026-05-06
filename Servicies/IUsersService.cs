using DTO_s;
using Enteties;

namespace Services
{
    public interface IUsersService
    {
        Task<AuthResponseDTO?> AddNewUser(UserDTO userDTO, string password);
        Task<AuthResponseDTO?> Login(ExisitingUser user);
        Task<bool> UpdateUser(int id, UserDTO userToUpdate, string password);
        Task<UserDTO> GetById(int id);
    }
}