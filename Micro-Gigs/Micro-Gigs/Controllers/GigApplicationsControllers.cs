using Micro_Gigs.DTOs;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GigApplicationsController : ControllerBase
    {
        private readonly GigApplicationsServices _service;

        public GigApplicationsController(GigApplicationsServices service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var apps = _service.GetAllApplications();
            return Ok(apps);
        }

        [HttpGet("GetById/{id:int}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var app = _service.GetApplicationById(id);
            if (app == null) return NotFound(new { message = $"Application with ID {id} was not found." });
            return Ok(app);
        }

        [HttpPost("Apply")]
        [Authorize(Roles = "Freelancer")]
        public async Task<IActionResult> Apply([FromBody] CreateGigApplicationDto dto)
        {
            // Get the freelancer ID from the JWT token instead of user input.
            int freelancerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var (success, error, appId) = await _service.CreateApplication(dto, freelancerId);

            if (!success) return BadRequest(new { message = error });
            return Ok(new { applicationId = appId });
        }

        [HttpPatch("UpdateStatus/{id:int}")]
        [Authorize(Roles = "Client")]
        public IActionResult UpdateStatus(int id, [FromBody] string status)
        {
            var success = _service.UpdateApplicationStatus(id, status);
            if (!success) return NotFound(new { message = $"Application with ID {id} was not found." });
            return NoContent();
        }

        [HttpDelete("Delete/{id:int}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var success = _service.DeleteApplication(id);
            if (!success) return NotFound(new { message = $"Application with ID {id} was not found." });
            return NoContent();
        }
    }
}