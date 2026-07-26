using Micro_Gigs.DTOs;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Micro_Gigs.DTOs.UsersInputDTOs;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("user")]
    public class UsersController : ControllerBase
    {
        private UsersServices usersService;


        // Constructor Injection: Inject UsersServices using Dependency Injection
        public UsersController(UsersServices _usersService)
        {
            usersService = _usersService;
        }


        // POST user/register
        // Public endpoint - No authentication required
        [HttpPost("Register")]
        public IActionResult Register(
            [FromBody] UsersInputDTOs.RegisterUserDto dto)
        {
            var user = usersService.Register(dto);


            if (user == null)
                return BadRequest(new
                {
                    message = "Email already exists."
                });


            return Ok(user);
        }



        // Login user and generate JWT token
        // Public endpoint - No authentication required
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDto dto)
        {
            LoginResponseDto result = usersService.Login(dto);


            if (result == null)
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });


            return Ok(result);
        }



        // Get all users
        // Protected endpoint - Authenticated users only
        [HttpGet("GetAll")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult GetAll()
        {
            var users = usersService.GetAll();

            return Ok(users);
        }





        // Get user details by ID
        // Protected endpoint - Authenticated users only
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





        // Update user information
        // Protected endpoint - Authenticated users only
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult Update(
            int id,
            [FromBody] UsersInputDTOs.UpdateUserDto dto)
        {
            var updatedUser = usersService.Update(id, dto);


            if (updatedUser == null)
                return NotFound(new
                {
                    message = $"User with ID {id} was not found."
                });


            return Ok(updatedUser);
        }





        // Delete user account
        // Protected endpoint - Authenticated users only
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult Delete(int id)
        {
            bool deleted = usersService.Delete(id);


            if (!deleted)
                return NotFound(new
                {
                    message = $"User with ID {id} was not found."
                });


            return NoContent();
        }
    }
}
