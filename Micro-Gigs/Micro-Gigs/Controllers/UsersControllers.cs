using Micro_Gigs.DTOs;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Micro_Gigs.DTOs.UsersInputDTOs;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private UsersServices usersService;
        private EmailService emailService;

        // Dependency Injection
        public UsersController(UsersServices _usersService, EmailService _emailService)
        {
            usersService = _usersService;
            emailService = _emailService;
        }

        // Register new user
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            var user = usersService.Register(dto);

            if (user == null)
                return BadRequest(new
                {
                    message = "Email already exists."
                });

            return Ok(user);
        }

        // Authenticate user and generate token
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDto dto)
        {
            var result = usersService.Login(dto);

            if (result == null)
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });

            return Ok(result);
        }

        // Retrieve all users 
        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var users = usersService.GetAll();
            return Ok(users);
        }

        // Retrieve user by ID 
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult GetById(int id)
        {
            var user = usersService.GetById(id);

            if (user == null)
                return NotFound(new
                {
                    message = $"User with ID {id} was not found."
                });

            return Ok(user);
        }

        // Update existing user information
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateUserDto dto)
        {
            var user = usersService.Update(id, dto);

            if (user == null)
                return NotFound(new
                {
                    message = $"User with ID {id} was not found."
                });

            return Ok(user);
        }

        // Delete user account 
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult Delete(int id)
        {
            var success = usersService.Delete(id);

            if (!success)
                return NotFound(new
                {
                    message = $"User with ID {id} was not found."
                });

            return NoContent();
        }

        // Send email to a user
        [HttpPost("SendEmail")]
        [Authorize(Roles = "Client,Freelancer")]
        public async Task<IActionResult> SendEmail([FromBody] UsersInputDTOs.SendEmailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool sent = await emailService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body);

            if (!sent)
                return StatusCode(500, new { message = "Failed to send email." });

            return Ok(new { message = "Email sent successfully." });
        }
    }
}