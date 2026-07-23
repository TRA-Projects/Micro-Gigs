using Micro_Gigs.Models;
using Microsoft.EntityFrameworkCore;

namespace Micro_Gigs.Repositories
{
    // هذا الكلاس مسؤول عن تنفيذ العمليات الخاصة بالمرفقات Attachments
    // ويطبق الـ Interface الخاص بالمرفقات IGigAttachmentsRepo
    public class GigAttachmentsRepo : IGigAttachmentsRepo
    {
        //// متغير يستخدم للوصول إلى قاعدة البيانات
        //// يحتوي على جميع DbSet الموجودة في MicroGigsContext
        //private readonly MicroGigsContext _db;

        //// Constructor
        //// يتم استدعاؤه عند إنشاء كائن من GigAttachmentsRepo
        //// ويستقبل MicroGigsContext للوصول إلى قاعدة البيانات
        //public GigAttachmentsRepo(MicroGigsContext db)
        //{
        //    _db = db;
        //}

        //// إضافة Attachment جديد إلى جدول Attachments
        //// AddAsync تضيف المرفق بشكل غير متزامن
        //public async Task AddAsync(GigAttachments attachment)
        //{
        //    await _db.Attachments.AddAsync(attachment);
        //}

        //// جلب جميع المرفقات المرتبطة بـ Gig معين
        //// يتم البحث باستخدام GigId
        //public async Task<List<GigAttachments>> GetByGigIdAsync(Guid gigId)
        //{
        //    return await _db.Attachments

        //        // البحث عن المرفقات التي يكون GigId الخاص بها
        //        // مساويًا للـ gigId الذي تم تمريره
        //        .Where(x => x.GigId == gigId)

        //        // تحويل النتائج إلى List
        //        .ToListAsync();
        //}

        //// البحث عن Attachment واحد باستخدام الـ ID
        //// إذا لم يتم العثور على المرفق يرجع null
        //public async Task<GigAttachments?> GetByIdAsync(Guid id)
        //{
        //    return await _db.Attachments.FindAsync(id);
        //}

        //// حذف Attachment من قاعدة البيانات
        //// Remove يقوم بتحديد المرفق للحذف
        //public void Delete(GigAttachments attachment)
        //{
        //    _db.Attachments.Remove(attachment);
        //}

        //// حفظ جميع التغييرات التي تمت على قاعدة البيانات
        //// إذا تم حفظ تغيير واحد أو أكثر يرجع true
        //// إذا لم يتم حفظ أي تغيير يرجع false
        //public async Task<bool> SaveChangesAsync()
        //{
        //    return (await _db.SaveChangesAsync()) > 0;
        //}
    }
}