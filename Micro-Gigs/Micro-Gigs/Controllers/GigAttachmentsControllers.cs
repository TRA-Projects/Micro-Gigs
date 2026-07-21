using Microsoft.AspNetCore.Mvc;
using Micro_Gigs.Services;
using Micro_Gigs.DTOs;

namespace Micro_Gigs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GigAttachmentsController : ControllerBase
    {
        private readonly GigAttachmentsService _service;

        public GigAttachmentsController(GigAttachmentsService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(GigAttachmentsInputDTOs input)
        {
            var userId = Guid.NewGuid(); //  حتى نربطه بنفاذ
            await _service.CreateAttachment(input, userId);
            return Ok("Upload successful.");
        }
    }
}