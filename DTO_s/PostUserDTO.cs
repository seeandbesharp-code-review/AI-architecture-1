using System.ComponentModel.DataAnnotations;

namespace DTO_s
{
    public record PostUserDTO(
        int Id,

        [EmailAddress]
        [Required]
        string Email,

        string FirstName,

        string LastName,

        [Required]
        string Password
    );
}
