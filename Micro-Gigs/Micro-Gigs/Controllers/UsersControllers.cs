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


        // Dependency Injection
        public UsersController(UsersServices _usersService)
        {
            usersService = _usersService;
        }



        // Register new user
        // Receives user data from request body
        [HttpPost("Register")]
        public IActionResult Register(
            [FromBody] RegisterUserDto dto)
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
        public IActionResult Login(
            [FromBody] LoginUserDto dto)
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
        // Requires authentication
        [HttpGet("GetAll")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult GetAll()
        {
            var users = usersService.GetAll();

            return Ok(users);
        }



        // Retrieve user by ID
        // ID received from query string
        [HttpGet("GetById")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult GetById(
            [FromQuery] int id)
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
        // ID from route and data from request body
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] UpdateUserDto dto)
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
        // ID received from query string
        [HttpDelete("Delete")]
        [Authorize(Roles = "Client,Freelancer")]
        public IActionResult Delete(
            [FromQuery] int id)
        {
            var success = usersService.Delete(id);



            if (!success)
                return NotFound(new
                {
                    message = $"User with ID {id} was not found."
                });



            return NoContent();
        }



        // Read custom value from request header
        [HttpGet("HeaderExample")]
        public IActionResult HeaderExample(
            [FromHeader(Name = "User-Type")] string userType)
        {
            return Ok(new
            {
                message = $"User type: {userType}"
            });
        }
    }
}
