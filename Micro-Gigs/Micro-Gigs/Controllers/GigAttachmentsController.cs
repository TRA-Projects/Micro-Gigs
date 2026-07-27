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
    }
}