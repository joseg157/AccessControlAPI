using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(
            100,
            MinimumLength = 3,
            ErrorMessage = "Username must be between 3 and 100 characters."
        )]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public required string PasswordHash { get; set; }

        // Optional properties
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }

        [MaxLength(250, ErrorMessage = "First name must be less than 250 characters.")]
        public string FirstName { get; set; } = "";

        [MaxLength(250, ErrorMessage = "Middle name must be less than 250 characters.")]
        public string MiddleName { get; set; } = "";

        [MaxLength(250, ErrorMessage = "Last name must be less than 250 characters.")]
        public string LastName { get; set; } = "";

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = "";

        public DateTimeOffset? Birthdate { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Updated at is required.")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        [Required(ErrorMessage = "Created at is required.")]
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    }
}
