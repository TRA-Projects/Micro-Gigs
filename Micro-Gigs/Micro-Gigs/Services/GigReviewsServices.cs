using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Micro_Gigs.DTOs;                             // للوصول إلى DTOs
using Micro_Gigs.Models;                           // للوصول إلى Models
using Micro_Gigs.Repositories.Implementations;     // للوصول إلى GigReviewsRepo

namespace Micro_Gigs.Services
{
    // =========================================================
    // GIG REVIEWS SERVICE
    // Service مسؤول عن العمليات الخاصة بتعقيبات وتقييمات الـ Gig
    // =========================================================

    public class GigReviewsServices
    {
        private readonly GigReviewsRepo _repository;
        private readonly MicroGigsContext _context; // تم استخدام اسم الـ Context الصحيح

        // =========================================================
        // CONSTRUCTOR
        // استقبال الـ Repo والـ Context عن طريق Dependency Injection
        // =========================================================

        public GigReviewsServices(GigReviewsRepo repository, MicroGigsContext context)
        {
            _repository = repository;
            _context = context;
        }

        // =========================================================
        // CREATE REVIEW
        // إنشاء تقييم جديد
        // =========================================================

        public async Task<GigReviews?> CreateReview(GigReviewsInputDTO input, int reviewerId)
        {
            // 1. التحقق من وجود الـ Assignment
            var assignment = await _context.Assignments
                .Include(a => a.Gig)
                .FirstOrDefaultAsync(a => a.AssignmentId == input.AssignmentId);

            if (assignment == null)
                throw new InvalidOperationException("Assignment not found.");

            // 2. التحقق من أن صاحب التقييم هو صاحب الـ Gig
            if (assignment.Gig?.ClientId != reviewerId)
                throw new UnauthorizedAccessException("You can only review assignments for your own gigs.");

            // 3. التحقق من حالة الـ Assignment
            if (assignment.Status != "Approved")
                throw new InvalidOperationException("Can only review completed assignments.");

            // 4. التحقق من عدم وجود تقييم سابق
            var existingReview = await _repository.GetByAssignmentIdAsync(input.AssignmentId);
            if (existingReview != null)
                throw new InvalidOperationException("Review already exists for this assignment.");

            // 5. إنشاء التقييم وتخزينه
            var review = new GigReviews
            {
                AssId = input.AssignmentId,
                ClientId = reviewerId,
                Rating = input.Rating,
                Comment = input.Comment
            };

            return await _repository.AddAsync(review);
        }
    }
}