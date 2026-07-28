using Microsoft.AspNetCore.Mvc;
using Micro_Gigs.DTOs;
using Micro_Gigs.Services;

namespace Micro_Gigs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GigApplicationsController : ControllerBase
    {
        private readonly GigApplicationsServices _applicationsService;

        public GigApplicationsController(GigApplicationsServices applicationsService)
        {
            _applicationsService = applicationsService;
        }

        // GET: api/GigApplications
        [HttpGet]
        public ActionResult<IEnumerable<GigApplicationDto>> GetAllApplications()
        {
            var applications = _applicationsService.GetAllApplications();
            return Ok(applications);
        }

        // GET: api/GigApplications/5
        [HttpGet("{id}")]
        public ActionResult<GigApplicationDto> GetApplicationById(int id)
        {
            var application = _applicationsService.GetApplicationById(id);
            if (application == null)
            {
                return NotFound(new { message = $"Application with ID {id} not found." });
            }
            return Ok(application);
        }

        // POST: api/GigApplications
        [HttpPost]
        public async Task<ActionResult<int>> CreateApplication([FromBody] CreateGigApplicationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = await _applicationsService.CreateApplication(dto);
            return CreatedAtAction(nameof(GetApplicationById), new { id = newId }, new { id = newId, message = "Application created successfully." });
        }

        // PATCH: api/GigApplications/5/status
        [HttpPatch("{id}/status")]
        public IActionResult UpdateApplicationStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest(new { message = "Status cannot be empty." });
            }

            bool updated = _applicationsService.UpdateApplicationStatus(id, status);
            if (!updated)
            {
                return NotFound(new { message = $"Application with ID {id} not found." });
            }

            return Ok(new { message = "Application status updated successfully." });
        }

        // DELETE: api/GigApplications/5
        [HttpDelete("{id}")]
        public IActionResult DeleteApplication(int id)
        {
            bool deleted = _applicationsService.DeleteApplication(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Application with ID {id} not found." });
            }

            return Ok(new { message = "Application deleted successfully (Soft Delete)." });
        }
    }
}