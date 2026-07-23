using Microsoft.EntityFrameworkCore;       // لاستخدام ToListAsync و FirstOrDefaultAsync و Include
using Micro_Gigs.Models;                   // للوصول إلى Model: GigReviews
using Micro_Gigs.Repositories.Interfaces; // للوصول إلى Interface: IGigReviewsRepository

namespace Micro_Gigs.Repositories.Implementations
{
    // Repository مسؤول عن التعامل مع بيانات GigReviews في قاعدة البيانات
    // ويطبق Interface: IGigReviewsRepository
    public class GigReviewsRepo : IGigReviewsRepository
    {
        // متغير خاص للوصول إلى قاعدة البيانات
        private readonly MicroGigsContext _context;

        // Constructor يستقبل MicroGigsContext عن طريق Dependency Injection
        public GigReviewsRepo(MicroGigsContext context)
        {
            // تخزين الـ Context داخل المتغير _context
            _context = context;
        }

        // =========================================================
        // GET ALL REVIEWS
        // جلب جميع التقييمات الموجودة في قاعدة البيانات
        // =========================================================
        public async Task<IEnumerable<GigReviews>> GetAllAsync()
        {
            // الوصول إلى جدول Reviews
            // Include(r => r.Assignment)
            // يجلب بيانات الـ Assignment المرتبط بكل Review
            //
            // Include(r => r.Reviewer)
            // يجلب بيانات المستخدم الذي قام بالتقييم
            //
            // ToListAsync()
            // تحويل النتائج إلى List بشكل غير متزامن
            return await _context.Reviews
                .Include(r => r.Assignment)
                .Include(r => r.Reviewer)
                .ToListAsync();
        }

        // =========================================================
        // GET REVIEW BY ID
        // جلب تقييم واحد باستخدام ReviewId
        // =========================================================
        public async Task<GigReviews?> GetByIdAsync(int reviewId)
        {
            // البحث عن Review يطابق الـ ID المطلوب
            //
            // Include(r => r.Assignment)
            // يجلب بيانات الـ Assignment المرتبط بالتقييم
            //
            // Include(r => r.Reviewer)
            // يجلب بيانات المستخدم الذي كتب التقييم
            //
            // FirstOrDefaultAsync()
            // يرجع أول عنصر مطابق
            // أو null إذا لم يتم العثور عليه
            return await _context.Reviews
                .Include(r => r.Assignment)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        // =========================================================
        // GET REVIEWS BY ASSIGNMENT ID
        // جلب جميع التقييمات الخاصة بـ Assignment معين
        // =========================================================
        public async Task<IEnumerable<GigReviews>> GetByAssignmentIdAsync(int assignmentId)
        {
            // البحث عن جميع Reviews
            // التي يكون AssignmentId فيها مساويًا للـ assignmentId المطلوب
            return await _context.Reviews
                .Where(r => r.AssignmentId == assignmentId)
                .ToListAsync();
        }

        // =========================================================
        // GET REVIEWS BY REVIEWER ID
        // جلب جميع التقييمات التي كتبها مستخدم معين
        // =========================================================
        public async Task<IEnumerable<GigReviews>> GetByReviewerIdAsync(int reviewerId)
        {
            // البحث عن جميع Reviews
            // التي يكون ReviewerId فيها مساويًا للـ reviewerId المطلوب
            return await _context.Reviews
                .Where(r => r.ReviewerId == reviewerId)
                .ToListAsync();
        }

        // =========================================================
        // ADD REVIEW
        // إضافة تقييم جديد إلى قاعدة البيانات
        // =========================================================
        public async Task<GigReviews> AddAsync(GigReviews review)
        {
            // إضافة الـ Review الجديد إلى جدول Reviews
            await _context.Reviews.AddAsync(review);

            // حفظ التغييرات بشكل فعلي في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع الـ Review الذي تمت إضافته
            return review;
        }

        // =========================================================
        // UPDATE REVIEW
        // تعديل Review موجود
        // =========================================================
        public async Task<GigReviews> UpdateAsync(GigReviews review)
        {
            // تحديد أن الـ Review تم تعديله
            _context.Reviews.Update(review);

            // حفظ التعديل في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع الـ Review بعد التعديل
            return review;
        }

        // =========================================================
        // DELETE REVIEW
        // حذف Review باستخدام ReviewId
        // =========================================================
        public async Task<bool> DeleteAsync(int reviewId)
        {
            // البحث عن Review باستخدام ReviewId
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            // إذا لم يتم العثور على Review
            if (review == null)
                return false; // إرجاع false يعني أن عملية الحذف لم تتم

            // حذف الـ Review من جدول Reviews
            _context.Reviews.Remove(review);

            // حفظ عملية الحذف في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع true يعني أن الحذف تم بنجاح
            return true;
        }
    }
}