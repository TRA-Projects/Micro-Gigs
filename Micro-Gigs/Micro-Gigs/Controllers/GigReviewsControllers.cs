using Microsoft.AspNetCore.Mvc;
using Micro_Gigs.DTOs;
using Micro_Gigs.Services;

namespace Micro_Gigs.Controllers
{
    // =========================================================
    // GIG REVIEWS CONTROLLER
    // Controller مسؤول عن التعامل مع طلبات GigReviews
    // =========================================================

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

        public GigReviewsController(
            GigReviewsServices service)
        {
            _service = service;
        }


        // =========================================================
        // CREATE REVIEW
        // POST: api/GigReviews
        // إنشاء Review جديد
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateReview(
            [FromBody] GigReviewsInputDTO input)
        {
            // -----------------------------------------------------
            // مؤقتاً نضع Reviewer ID = 1
            // لاحقاً يتم أخذ User ID من JWT Token
            // -----------------------------------------------------

            int reviewerId = 1;

            // -----------------------------------------------------
            // استدعاء Service لإنشاء Review
            // -----------------------------------------------------

            var review =
                await _service.CreateReview(input, reviewerId);

            // -----------------------------------------------------
            // إرجاع البيانات بعد الإنشاء
            // HTTP 200 OK
            // -----------------------------------------------------

            return Ok(review);
        }
    }
}