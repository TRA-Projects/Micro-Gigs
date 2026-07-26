using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories
{
    //// هذا الـ Interface يحدد العمليات الخاصة بالتقييمات Reviews
    //// أي Repository خاص بالتقييمات يجب أن يطبق هذه العمليات
    //public interface IGigReviewsRepo
    //{
    //    // إضافة تقييم جديد إلى قاعدة البيانات
    //    Task AddAsync(GigReviews review);

    //    // جلب جميع التقييمات المرتبطة بتكليف معين Assignment
    //    Task<List<GigReviews>> GetByAssignmentIdAsync(Guid assignmentId);

    //    // البحث عن تقييم معين باستخدام الـ ID
    //    // إذا لم يتم العثور على التقييم يرجع null
    //    Task<GigReviews?> GetByIdAsync(Guid id);

    //    // حفظ جميع التغييرات التي تمت على قاعدة البيانات
    //    // يرجع true إذا تم حفظ تغيير واحد أو أكثر
    //    Task<bool> SaveChangesAsync();
    //}
}