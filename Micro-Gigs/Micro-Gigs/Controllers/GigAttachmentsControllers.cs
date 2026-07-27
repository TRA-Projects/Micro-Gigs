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
        // UPLOAD FILE & CREATE ATTACHMENT
        // POST: api/GigAttachments/upload
        // رفع ملف إلى السيرفر وإنشاء Attachment جديد تلقائياً
        // =========================================================
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] int gigId)
        {
            // =====================================================
            // 1. VALIDATION
            // التحقق من وجود الملف المرفوع وحجمه
            // =====================================================
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }


            // =====================================================
            // 2. USER ID FROM TOKEN
            // استخراج رقم المستخدم تلقائياً من الـ JWT Token
            // =====================================================
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Unable to retrieve user ID from token." });
            }


            // =====================================================
            // 3. CREATE DIRECTORY
            // إنشاء مجلد الحفظ في حال عدم وجوده
            // =====================================================
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            // =====================================================
            // 4. SAVE FILE TO DISK
            // إنشاء اسم فريد للملف وتخزينه على السيرفر
            // =====================================================
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            // =====================================================
            // 5. CALL SERVICE
            // تجهيز مسار الملف وإنشاء Attachment جديد عبر الـ Service
            // =====================================================
            var fileUrl = $"/uploads/{fileName}";
            var input = new GigAttachmentsInputDTO
            {
                GigId = gigId,
                FileUrl = fileUrl
            };

            var attachment = await _service.CreateAttachment(input, userId);


            // =====================================================
            // 6. RETURN RESULT
            // إرجاع البيانات التي تمت إضافتها (HTTP 200 OK)
            // =====================================================
            return Ok(attachment);
        }
    }
}