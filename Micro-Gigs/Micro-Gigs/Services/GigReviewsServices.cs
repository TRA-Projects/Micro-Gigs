using Micro_Gigs.Models;                           
using Micro_Gigs.DTOs;                             
using Micro_Gigs.Repositories.Implementations;    

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
        // إنشاء متغير للوصول إلى Repository المباشر
        // =========================================================

        private readonly GigReviewsRepo _repository;


        // =========================================================
        // CONSTRUCTOR
        // استقبال Repository عن طريق Dependency Injection
        // =========================================================

        public GigReviewsServices(GigReviewsRepo repository)
        {
            // تخزين الـ Repository داخل المتغير _repository
            _repository = repository;
        }


        // =========================================================
        // CREATE REVIEW
        // إنشاء تقييم جديد
        // =========================================================

        public async Task<GigReviewsOutputDTO> CreateReview(
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
                // أخذ رقم الـ Assignment من الـ DTO وربط التقييم بالتكليف
                // =================================================

                AssignmentId = input.AssignmentId,


                // =================================================
                // REVIEWER ID
                // تحديد المستخدم الذي قام بكتابة التقييم (المأخوذ من Token)
                // =================================================

                ClientId = reviewerId,

                // =================================================
                // RATING
                // أخذ درجة التقييم من الـ DTO (من 1 إلى 5)
                // =================================================

                Rating = input.Rating,


                // =================================================
                // COMMENT
                // أخذ التعليق من الـ DTO
                // =================================================

                Comment = input.Comment
            };


            // =====================================================
            // ADD REVIEW
            // إرسال الـ Review إلى Repository لإضافته وحفظه في قاعدة البيانات
            // =====================================================

            var created = await _repository.AddAsync(review);

            return new GigReviewsOutputDTO
            {
                ReviewId = created.ReviewId,
                AssignmentId = created.AssignmentId,
                ReviewerId = created.ClientId,
                Rating = created.Rating,
                Comment = created.Comment
            };
        }

        // =========================================================
        // READ - ALL / BY ID / BY ASSIGNMENT / BY REVIEWER
        // =========================================================
        public async Task<IEnumerable<GigReviewsOutputDTO>> GetAllReviews()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(r => new GigReviewsOutputDTO
            {
                ReviewId = r.ReviewId,
                AssignmentId = r.AssignmentId,
                ReviewerId = r.ClientId,
                Rating = r.Rating,
                Comment = r.Comment
            });
        }

        public async Task<GigReviewsOutputDTO?> GetById(int reviewId)
        {
            var r = await _repository.GetByIdAsync(reviewId);
            if (r == null) return null;
            return new GigReviewsOutputDTO
            {
                ReviewId = r.ReviewId,
                AssignmentId = r.AssignmentId,
                ReviewerId = r.ClientId,
                Rating = r.Rating,
                Comment = r.Comment
            };
        }

        public async Task<IEnumerable<GigReviewsOutputDTO>> GetByAssignmentId(int assignmentId)
        {
            var list = await _repository.GetByAssignmentIdAsync(assignmentId);
            return list.Select(r => new GigReviewsOutputDTO
            {
                ReviewId = r.ReviewId,
                AssignmentId = r.AssignmentId,
                ReviewerId = r.ClientId,
                Rating = r.Rating,
                Comment = r.Comment
            });
        }

        public async Task<IEnumerable<GigReviewsOutputDTO>> GetByReviewerId(int reviewerId)
        {
            var list = await _repository.GetByReviewerIdAsync(reviewerId);
            return list.Select(r => new GigReviewsOutputDTO
            {
                ReviewId = r.ReviewId,
                AssignmentId = r.AssignmentId,
                ReviewerId = r.ClientId,
                Rating = r.Rating,
                Comment = r.Comment
            });
        }

        // =========================================================
        // UPDATE REVIEW
        // Only the original reviewer (ClientId) can update their review
        // =========================================================
        public async Task<GigReviewsOutputDTO?> UpdateReview(int reviewId, GigReviewsInputDTO input, int reviewerId)
        {
            var review = await _repository.GetByIdAsync(reviewId);
            if (review == null) return null;
            if (review.ClientId != reviewerId) return null; // not owner

            review.Rating = input.Rating;
            review.Comment = input.Comment;

            var updated = await _repository.UpdateAsync(review);
            return new GigReviewsOutputDTO
            {
                ReviewId = updated.ReviewId,
                AssignmentId = updated.AssignmentId,
                ReviewerId = updated.ClientId,
                Rating = updated.Rating,
                Comment = updated.Comment
            };
        }

        // =========================================================
        // DELETE REVIEW
        // Only the original reviewer can delete
        // =========================================================
        public async Task<bool> DeleteReview(int reviewId, int reviewerId)
        {
            var review = await _repository.GetByIdAsync(reviewId);
            if (review == null) return false;
            if (review.ClientId != reviewerId) return false;

            return await _repository.DeleteAsync(reviewId);
        }
    }
}