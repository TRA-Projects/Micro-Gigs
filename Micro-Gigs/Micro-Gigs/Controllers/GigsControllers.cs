using Micro_Gigs.DTOs;
using Micro_Gigs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GigsControllers : ControllerBase
    {
        private GigsServices gigsServices;

        public GigsControllers(GigsServices _gigsServices)
        {
            gigsServices = _gigsServices;
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

        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] int id)
        {
            var gig = gigsServices.GetGigById(id);
            if (gig == null) return NotFound();
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
            if (gig == null) return BadRequest(new { massage = "Only client can create gigs" });

            return CreatedAtAction(nameof(GetById), new { id = gig.GigId }, gig);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Client")]
        public IActionResult Update([FromBody] int id, [FromBody] CreateGigDto dto)
        {
            var success = gigsServices.UpdateGig(id, dto);
            if (!success) return NotFound();

            return NoContent();
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "Client")]
        public IActionResult Delete([FromBody] int id)
        {
            var success = gigsServices.DeleteGig(id);
            if (!success) return NotFound();

            return NoContent();
        }


    }
}