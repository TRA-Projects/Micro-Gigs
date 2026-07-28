using Microsoft.AspNetCore.Mvc;        
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims;         
using Micro_Gigs.Services;            
using Micro_Gigs.DTOs;             
namespace Micro_Gigs.Controllers
{
    // =========================================================
    // GIG ATTACHMENTS CONTROLLER
    // Controller مسؤول عن التعامل مع طلبات GigAttachments
    // =========================================================

    [Authorize] // حماية الـ Controller لضمان وجود JWT Token صالحة
    [ApiController] // تحديد أن هذا Controller خاص بـ Web API
    [Route("api/[controller]")] // تحديد المسار الأساسي: api/GigAttachments
    public class GigAttachmentsController : ControllerBase
    {
        // =========================================================
        // SERVICE
        // إنشاء متغير للوصول إلى GigAttachmentsServices
        // =========================================================
        private readonly GigAttachmentsServices _service;


        // =========================================================
        // CONSTRUCTOR
        // استقبال الـ Service عن طريق Dependency Injection
        // =========================================================
        public GigAttachmentsController(GigAttachmentsServices service)
        {
            // تخزين الـ Service داخل المتغير _service
            _service = service;
        }


        // =========================================================
        // CREATE ATTACHMENT
        // POST: api/GigAttachments
        // إنشاء Attachment جديد
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> CreateAttachment([FromBody] GigAttachmentsInputDTO input)
        {
            // =====================================================
            // USER ID FROM TOKEN
            // استخراج رقم المستخدم تلقائياً من الـ JWT Token
            // =====================================================
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            // التحقق من وجود القيمة وإمكانية تحويلها إلى رقم صحفي (int)
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Unable to retrieve user ID from token. Please write to me.." });
            }

            // =====================================================
            // CALL SERVICE
            // استدعاء Service لإنشاء Attachment جديد مع تمرير userId المستخرج
            // =====================================================
            var attachment = await _service.CreateAttachment(input, userId);


            // =====================================================
            // RETURN RESULT
            // إرجاع البيانات التي تمت إضافتها
            // HTTP 200 OK
            // =====================================================
            return Ok(attachment);
        }

        // =========================================================
        // UPLOAD FILE
        // POST: api/GigAttachments/upload
        // multipart/form-data with fields: GigId, File
        // =========================================================
        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50 MB
        public async Task<IActionResult> Upload([FromForm] GigAttachmentUploadDTO input)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Unable to retrieve user ID from token." });

            if (input.File == null || input.File.Length == 0)
                return BadRequest(new { message = "File is required." });

            var created = await _service.CreateAttachmentFromFile(input.File, input.GigId, userId);
            return Ok(created);
        }

        // =========================================================
        // GET ALL
        // GET: api/GigAttachments
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAttachments();
            return Ok(items);
        }

        // =========================================================
        // GET BY ID
        // GET: api/GigAttachments/{id}
        // =========================================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // =========================================================
        // UPDATE
        // PUT: api/GigAttachments/{id}
        // =========================================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GigAttachmentsInputDTO input)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Unable to retrieve user ID from token." });

            var updated = await _service.UpdateAttachment(id, input, userId);
            if (updated == null) return Forbid();
            return Ok(updated);
        }

        // =========================================================
        // DELETE
        // DELETE: api/GigAttachments/{id}
        // =========================================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Unable to retrieve user ID from token." });

            var deleted = await _service.DeleteAttachment(id, userId);
            if (!deleted) return Forbid();
            return Ok(new { success = true });
        }
    }
}