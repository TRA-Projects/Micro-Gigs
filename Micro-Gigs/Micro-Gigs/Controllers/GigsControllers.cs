using Micro_Gigs.DTOs;
using Micro_Gigs.Repositories;
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
        private GigsRepo gigsRepo;

        public GigsControllers(GigsServices _gigsServices, GigsRepo _gigsRepo) 
        {
            gigsServices = _gigsServices;
            gigsRepo = _gigsRepo; // assign injected repo
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
        public IActionResult Update([FromQuery] int id, [FromBody] CreateGigDto dto)
        {
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var gig = gigsServices.GetGigById(id);
            if (gig == null) return NotFound();

            //  التحقق من الملكية
            var fullGig = gigsRepo.GetById(id); // use instance
            if (fullGig == null || fullGig.ClientId != clientId)
                return Forbid();  // 403

            var success = gigsServices.UpdateGig(id, dto);
            if (!success) return NotFound();

            return NoContent();
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "Client")]
        public IActionResult Delete([FromQuery] int id)
        {
            int clientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var fullGig = gigsRepo.GetById(id);
            if (fullGig == null) return NotFound();

            //  التحقق من الملكية
            if (fullGig.ClientId != clientId)
                return Forbid();


            var success = gigsServices.DeleteGig(id);
            if (!success) return NotFound();

            return NoContent();
        }


    }
}