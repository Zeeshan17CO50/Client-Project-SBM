using Client.Application.Features.Role.Dtos;
using Client.Application.Features.User.Dtos;

namespace Client_WebApp.Models.Master
{
    public class UserViewModel
    {
        public int CompanyId { get; set; }
        public UserDto User { get; set; }
        public List<UserDto> Users { get; set; }
        public List<RoleDto> Roles { get; set; } // for dropdown

        public UserViewModel()
        {
            User = new UserDto();
            Users = new List<UserDto>();
            Roles = new List<RoleDto>();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int RoleMasterId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public int IsActive { get; set; } = 1;
    }
}
