using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories
{
    // هذا الـ Interface يحدد العمليات الخاصة بالمرفقات Attachments
    // أي Repository خاص بالمرفقات يجب أن يطبق هذه العمليات
    public interface IGigAttachmentsRepo
    {
        // إضافة مرفق جديد إلى قاعدة البيانات
        Task AddAsync(GigAttachments attachment);

        // جلب جميع المرفقات الخاصة بـ Gig معين
        // يتم البحث باستخدام GigId
        Task<List<GigAttachments>> GetByGigIdAsync(Guid gigId);

        // البحث عن مرفق معين باستخدام الـ ID
        // إذا لم يتم العثور على المرفق يرجع null
        Task<GigAttachments?> GetByIdAsync(Guid id);

        // حذف مرفق من قاعدة البيانات
        void Delete(GigAttachments attachment);

        // حفظ جميع التغييرات التي تمت على قاعدة البيانات
        // يرجع true إذا تم حفظ تغيير واحد أو أكثر
        Task<bool> SaveChangesAsync();
    }
}