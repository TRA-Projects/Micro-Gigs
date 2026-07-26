using Microsoft.AspNetCore.Mvc;
// يحتوي على ControllerBase و HttpPost و HttpGet و HttpPut و HttpDelete

using Micro_Gigs.Services;
// للوصول إلى Service:
// GigAttachmentsServices

using Micro_Gigs.DTOs;
// للوصول إلى DTOs:
// GigAttachmentsInputDTO
// GigAttachmentsOutputDTO


namespace Micro_Gigs.Controllers
{
    // =========================================================
    // GIG ATTACHMENTS CONTROLLER
    // Controller مسؤول عن التعامل مع طلبات GigAttachments
    // =========================================================

    [ApiController]
    // تحديد أن هذا Controller خاص بـ Web API

    [Route("api/[controller]")]
    // تحديد المسار الأساسي للـ Controller
    // مثال:
    // api/GigAttachments

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

        public GigAttachmentsController(
            GigAttachmentsServices service)
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
        public async Task<IActionResult> CreateAttachment(
            [FromBody] GigAttachmentsInputDTO input)
        {
            // =====================================================
            // USER ID
            // تحديد رقم المستخدم الذي قام برفع الملف
            // مؤقتاً نستخدم User ID = 1
            // لاحقاً يمكن أخذه من JWT Token
            // =====================================================

            int userId = 1;


            // =====================================================
            // CALL SERVICE
            // استدعاء Service لإنشاء Attachment جديد
            // =====================================================

            var attachment =
                await _service.CreateAttachment(input, userId);


            // =====================================================
            // RETURN RESULT
            // إرجاع البيانات التي تمت إضافتها
            // HTTP 200 OK
            // =====================================================

            return Ok(attachment);
        }
    }
}