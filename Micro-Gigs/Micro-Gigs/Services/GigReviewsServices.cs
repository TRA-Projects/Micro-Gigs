using Micro_Gigs.Models;
// للوصول إلى Model:
// GigReviews

using Micro_Gigs.DTOs;
// للوصول إلى DTOs الخاصة بالـ Reviews

using Micro_Gigs.Repositories.Interfaces;
// للوصول إلى Interface:
// IGigReviewsRepository


namespace Micro_Gigs.Services
{
    // =========================================================
    // GIG REVIEWS SERVICE
    // Service مسؤول عن العمليات الخاصة بتقييمات الـ Gig
    // =========================================================

    public class GigReviewsServices
    {
        // =========================================================
        // REPOSITORY
        // إنشاء متغير للوصول إلى Repository
        // =========================================================

        private readonly IGigReviewsRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // استقبال Repository عن طريق Dependency Injection
        // =========================================================

        public GigReviewsServices(
            IGigReviewsRepository repository)
        {
            // تخزين الـ Repository داخل المتغير _repository
            _repository = repository;
        }


        // =========================================================
        // CREATE REVIEW
        // إنشاء تقييم جديد
        // =========================================================

        public async Task<GigReviews> CreateReview(
            GigReviewsInputDTO input,
            int reviewerId)
        {
            // =====================================================
            // CREATE NEW REVIEW
            // إنشاء Object جديد من Model: GigReviews
            // =====================================================

            var review = new GigReviews
            {
                // =================================================
                // ASSIGNMENT ID
                // أخذ رقم الـ Assignment من الـ DTO
                // وربط التقييم بالتكليف
                // =================================================

                AssignmentId = input.AssignmentId,


                // =================================================
                // REVIEWER ID
                // تحديد المستخدم الذي قام بكتابة التقييم
                // يتم أخذ User ID من الـ Token
                // =================================================

                ReviewerId = reviewerId,


                // =================================================
                // RATING
                // أخذ درجة التقييم من الـ DTO
                // القيمة يجب أن تكون من 1 إلى 5
                // =================================================

                Rating = input.Rating,


                // =================================================
                // COMMENT
                // أخذ التعليق من الـ DTO
                // التعليق اختياري ويمكن أن يكون null
                // =================================================

                Comment = input.Comment
            };


            // =====================================================
            // ADD REVIEW
            // إرسال الـ Review إلى Repository
            // لإضافته وحفظه في قاعدة البيانات
            // =====================================================

            return await _repository.AddAsync(review);
        }
    }
}