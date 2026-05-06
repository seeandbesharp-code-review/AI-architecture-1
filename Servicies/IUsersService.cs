using DTO_s;
using Enteties;

namespace Services
{
    public interface IUsersService
    {
        Task<AuthResponseDTO?> AddNewUser(PostUserDTO userDTO);
        Task<AuthResponseDTO?> Login(ExisitingUser user);
        Task<bool> UpdateUser(int id, PostUserDTO userToUpdate);
        Task<UserDTO> GetById(int id);
    }
}