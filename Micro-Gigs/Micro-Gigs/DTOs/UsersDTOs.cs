using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    public class UsersInputDTOs
    {
        // Register
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Type is required")]
        [MaxLength(20)]
        public string UserType { get; set; } = string.Empty; // Client or Freelancer
    }

    public class UsersOutputDTOs
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserType { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
    }
}
