using Micro_Gigs.DTOs;
using Micro_Gigs.Repositories;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GigsController : ControllerBase
    {
        private readonly GigsServices gigsServices;
        private readonly GigsRepo gigsRepo;

        public GigsController(GigsServices _gigsServices, GigsRepo _gigsRepo)
        {
            gigsServices = _gigsServices;
            gigsRepo = _gigsRepo;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var gigs = gigsServices.GetAllGigs();
            return Ok(gigs);
        }

        [HttpGet("GetOpen")]
        public IActionResult GetOpen()
        {
            var gigs = gigsServices.GetOpenGigs();
            return Ok(gigs);
        }

        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var gig = gigsServices.GetGigById(id);
            if (gig == null) return NotFound(new { message = $"Gig with ID {id} was not found." });
            return Ok(gig);
        }

        [HttpGet("GetByClient")]
        public IActionResult GetByClient([FromQuery] int id)
        {
            var gigs = gigsServices.GetGigsByClient(id);
            return Ok(gigs);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Client")]
        public IActionResult Create([FromBody] CreateGigDto dto)
        {
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var gig = gigsServices.CreateGig(dto, clientId);
            if (gig == null) return BadRequest(new { message = "Only clients can create gigs." });

            return CreatedAtAction(nameof(GetById), new { id = gig.GigId }, gig);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Client")]
        public IActionResult Update([FromQuery] int id, [FromBody] CreateGigDto dto)
        {
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var fullGig = gigsRepo.GetById(id);
            if (fullGig == null) return NotFound(new { message = $"Gig with ID {id} was not found." });

            if (fullGig.ClientId != clientId)
                return Forbid();

            var success = gigsServices.UpdateGig(id, dto, clientId);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Client")]
        public IActionResult Delete([FromQuery] int id)
        {
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var fullGig = gigsRepo.GetById(id);
            if (fullGig == null) return NotFound(new { message = $"Gig with ID {id} was not found." });

            if (fullGig.ClientId != clientId)
                return Forbid();

            var success = gigsServices.DeleteGig(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}