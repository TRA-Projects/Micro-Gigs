using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    public class UsersInputDTOs
    {
        // =========================================================
        // Request DTOs - Data received from the client
        // =========================================================


        // DTO used when creating a new user account
        public class RegisterUserDto
        {
            // User's display name
            [Required(ErrorMessage = "Username is required.")]
            [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
            public string UserName { get; set; } = string.Empty;


            // User's email address used for registration and authentication
            [Required(ErrorMessage = "Email is required.")]
            [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; } = string.Empty;


            // User password that will be hashed before storing in the database
            [Required(ErrorMessage = "Password is required.")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
            public string Password { get; set; } = string.Empty;


            // Defines the user role in the system (Client or Freelancer)
            [Required(ErrorMessage = "User type is required.")]
            [MaxLength(20, ErrorMessage = "User type cannot exceed 20 characters.")]
            public string UserType { get; set; } = string.Empty;
        }


        // DTO used to send an email to a user
        public class SendEmailDto
        {
            // Recipient email address
            [Required(ErrorMessage = "Recipient email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string ToEmail { get; set; } = string.Empty;

            // Email subject
            [Required(ErrorMessage = "Subject is required.")]
            public string Subject { get; set; } = string.Empty;

            // Email body/content
            [Required(ErrorMessage = "Body is required.")]
            public string Body { get; set; } = string.Empty;
        }




        // DTO used for user authentication (Login)
        public class LoginUserDto
        {
            // Email used to identify the user account
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; } = string.Empty;


            // User password used for verification
            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; } = string.Empty;
        }




        // DTO used to update existing user information
        public class UpdateUserDto
        {
            // Updated username
            [Required(ErrorMessage = "Username is required.")]
            [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
            public string UserName { get; set; } = string.Empty;


            // Updated email address
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
            public string Email { get; set; } = string.Empty;


            // Updated user type (Client or Freelancer)
            [Required(ErrorMessage = "User type is required.")]
            [MaxLength(20, ErrorMessage = "User type cannot exceed 20 characters.")]
            public string UserType { get; set; } = string.Empty;
        }




        // =========================================================
        // Response DTOs - Data returned from the API
        // =========================================================


        // DTO used to return user information to the client
        public class UsersResponseDto
        {
            // Unique identifier of the user
            public int UserId { get; set; }


            // User's display name
            public string UserName { get; set; } = string.Empty;


            // User's email address
            public string Email { get; set; } = string.Empty;


            // User role/type in the system
            public string UserType { get; set; } = string.Empty;


            // Date when the account was created
            public DateTime RegistrationDate { get; set; }

        }


        // DTO returned after successful authentication
        public class LoginResponseDto
        {
            // JWT token used for authenticated requests
            public string Token { get; set; } = string.Empty;


            // Logged-in user's identifier
            public int UserId { get; set; }


            // Logged-in user's name
            public string UserName { get; set; } = string.Empty;


            // Logged-in user's type (Client or Freelancer)
            public string UserType { get; set; } = string.Empty;
        }
    }
}