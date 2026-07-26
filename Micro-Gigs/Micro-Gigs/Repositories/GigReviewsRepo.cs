using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs.Repositories
//{
//    // هذا الكلاس مسؤول عن تنفيذ العمليات الخاصة بالتقييمات Reviews
//    // ويطبق الـ Interface IGigReviewsRepo
//    public class GigReviewsRepo : IGigReviewsRepo
//    {
//        // متغير خاص للوصول إلى قاعدة البيانات
//        // readonly يعني لا يمكن تغيير قيمته بعد إنشاء الكلاس
//        private readonly MicroGigsContext _db;

//        // Constructor
//        // يتم استدعاؤه عند إنشاء كائن من GigReviewsRepo
//        // ويستقبل MicroGigsContext للوصول إلى قاعدة البيانات
//        public GigReviewsRepo(MicroGigsContext db)
//        {
//            _db = db;
//        }

//        // إضافة Review جديد إلى جدول Reviews
//        // AddAsync تضيف العنصر بشكل غير متزامن
//        public async Task AddAsync(GigReviews review)
//        {
//            await _db.Reviews.AddAsync(review);
//        }

//        // جلب جميع Reviews المرتبطة بـ Assignment معين
//        // يتم البحث باستخدام AssignmentId
//        public async Task<List<GigReviews>> GetByAssignmentIdAsync(Guid assignmentId)
//        {
//            return await _db.Reviews
//                // البحث عن التقييمات التي يكون AssignmentId الخاص بها
//                // مساويًا للـ assignmentId الذي تم تمريره
//                .Where(x => x.AssignmentId == assignmentId)

//                // تحويل النتائج إلى List
//                .ToListAsync();
//        }

//        // البحث عن Review واحد باستخدام الـ ID
//        // إذا لم يتم العثور عليه يرجع null
//        public async Task<GigReviews?> GetByIdAsync(Guid id)
//        {
//            return await _db.Reviews.FindAsync(id);
//        }

//        // حفظ جميع التغييرات التي تمت على قاعدة البيانات
//        // إذا تم حفظ تغيير واحد أو أكثر يرجع true
//        // إذا لم يتم حفظ أي تغيير يرجع false
//        public async Task<bool> SaveChangesAsync()
//        {
//            return (await _db.SaveChangesAsync()) > 0;
//        }
    }
}
