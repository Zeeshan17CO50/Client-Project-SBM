using Client.Application.Features.Role.Dtos;
using Client.Application.Features.User.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Client_WebApp.Models.Master
{
    public class UserViewModel
    {
        public int CompanyId { get; set; }
        public User User { get; set; }
        public List<UserDto> Users { get; set; }
        public List<RoleDto> Roles { get; set; } // for dropdown

        public UserViewModel()
        {
            User = new User();
            Users = new List<UserDto>();
            Roles = new List<RoleDto>();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int RoleMasterId { get; set; }
        [Required(ErrorMessage = "*Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "*Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&\-+=()])(?=\S+$).{4,10}$",
        ErrorMessage = "Password must have 1 capital, 1 small, 1 number, 1 special char, 4-10 chars.")]
        public string? Password { get; set; }
        public int IsActive { get; set; } = 1;
    }
}
