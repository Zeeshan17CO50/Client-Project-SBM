using System.ComponentModel.DataAnnotations;

namespace Client_WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&\-+=()])(?=\S+$).{4,10}$",
            ErrorMessage = "Password must have 1 capital, 1 small, 1 number, 1 special char, 4-10 chars.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please verify you are not a robot.")]
        public string RecaptchaToken { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    public class JwtClaims
    {
        public string User { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public int CompanyID { get; set; }
        public string Email { get; set; }
    }
}
