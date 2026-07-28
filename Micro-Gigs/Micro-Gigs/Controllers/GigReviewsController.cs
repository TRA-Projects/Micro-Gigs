using Microsoft.AspNetCore.Mvc;        // يحتوي على ControllerBase و HttpPost و HttpGet إلخ
using Microsoft.AspNetCore.Authorization; // لاستخدام أتربيوت [Authorize]
using System.Security.Claims;          // لاستخراج البيانات من الـ JWT Claims
using Micro_Gigs.DTOs;                // للوصول إلى DTOs
using Micro_Gigs.Services;            // للوصول إلى Services

namespace Micro_Gigs.Controllers
{
    // =========================================================
    // GIG REVIEWS CONTROLLER
    // Controller مسؤول عن التعامل مع طلبات GigReviews
    // =========================================================

    [Authorize] // حماية الـ Controller لضمان وجود JWT Token صالحة
    [ApiController]
    [Route("api/[controller]")]
    public class GigReviewsController : ControllerBase
    {
        // =========================================================
        // SERVICE
        // إنشاء متغير للوصول إلى GigReviewsServices
        // =========================================================

        private readonly GigReviewsServices _service;


        // =========================================================
        // CONSTRUCTOR
        // استقبال Service عن طريق Dependency Injection
        // =========================================================

        public GigReviewsController(GigReviewsServices service)
        {
            _service = service;
        }


        // =========================================================
        // CREATE REVIEW
        // POST: api/GigReviews
        // إنشاء Review جديد
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] GigReviewsInputDTO input)
        {
            // -----------------------------------------------------
            // استخراج Reviewer ID تلقائياً من الـ JWT Token
            // -----------------------------------------------------
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int reviewerId))
            {
                return Unauthorized(new { message = "Unable to retrieve user ID from token. Please write to me." });
            }

            // -----------------------------------------------------
            // استدعاء Service لإنشاء Review مع تمرير reviewerId المستخرج
            // -----------------------------------------------------

            var review = await _service.CreateReview(input, reviewerId);

            // -----------------------------------------------------
            // إرجاع البيانات بعد الإنشاء
            // HTTP 200 OK
            // -----------------------------------------------------

            return Ok(review);
        }

        // =========================================================
        // GET ALL REVIEWS
        // GET: api/GigReviews
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _service.GetAllReviews();
            return Ok(reviews);
        }

        // =========================================================
        // GET BY ID
        // GET: api/GigReviews/{id}
        // =========================================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _service.GetById(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        // =========================================================
        // UPDATE
        // PUT: api/GigReviews/{id}
        // =========================================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GigReviewsInputDTO input)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int reviewerId))
            {
                return Unauthorized(new { message = "Unable to retrieve user ID from token." });
            }

            var updated = await _service.UpdateReview(id, input, reviewerId);
            if (updated == null) return Forbid();
            return Ok(updated);
        }

        // =========================================================
        // DELETE
        // DELETE: api/GigReviews/{id}
        // =========================================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int reviewerId))
            {
                return Unauthorized(new { message = "Unable to retrieve user ID from token." });
            }

            var deleted = await _service.DeleteReview(id, reviewerId);
            if (!deleted) return Forbid();
            return Ok(new { success = true });
        }
    }
}