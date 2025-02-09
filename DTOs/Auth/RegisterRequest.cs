using System.ComponentModel.DataAnnotations;

namespace server.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}
