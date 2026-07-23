using Microsoft.EntityFrameworkCore;       // لاستخدام ToListAsync و FirstOrDefaultAsync
using Micro_Gigs.Models;                   // للوصول إلى Model: GigAttachments
using Micro_Gigs.Repositories.Interfaces; // للوصول إلى Interface: IGigAttachmentsRepository

namespace Micro_Gigs.Repositories.Implementations
{
    // Repository مسؤول عن التعامل مع بيانات GigAttachments في قاعدة البيانات
    // ويطبق Interface: IGigAttachmentsRepository
    public class GigAttachmentsRepo : IGigAttachmentsRepository
    {
        // إنشاء متغير خاص للوصول إلى قاعدة البيانات
        private readonly MicroGigsContext _context;

        // Constructor يستقبل MicroGigsContext عن طريق Dependency Injection
        public GigAttachmentsRepo(MicroGigsContext context)
        {
            // تخزين الـ Context داخل المتغير _context
            _context = context;
        }

        // =========================================================
        // GET ALL
        // جلب جميع الـ Attachments الموجودة في قاعدة البيانات
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetAllAsync()
        {
            // الوصول إلى جدول Attachments
            // ToListAsync() ترجع جميع البيانات الموجودة في الجدول
            return await _context.Attachments
                .ToListAsync();
        }

        // =========================================================
        // GET BY ID
        // جلب Attachment واحد باستخدام AttachmentId
        // =========================================================
        public async Task<GigAttachments?> GetByIdAsync(int attachmentId)
        {
            // البحث عن Attachment يطابق الـ ID الذي أرسله المستخدم
            // FirstOrDefaultAsync ترجع العنصر إذا وجدته
            // أو null إذا لم تجده
            return await _context.Attachments
                .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId);
        }

        // =========================================================
        // GET BY GIG ID
        // جلب جميع الملفات المرتبطة بـ Gig معين
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetByGigIdAsync(int gigId)
        {
            // البحث عن جميع الـ Attachments
            // التي يكون GigId الخاص بها مساويًا للـ gigId المطلوب
            return await _context.Attachments
                .Where(a => a.GigId == gigId)
                .ToListAsync();
        }

        // =========================================================
        // GET BY USER ID
        // جلب جميع الملفات التي رفعها User معين
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetByUserIdAsync(int userId)
        {
            // البحث عن جميع الـ Attachments
            // التي يكون UserID الخاص بها مساويًا للـ userId المطلوب
            return await _context.Attachments
                .Where(a => a.UserID == userId)
                .ToListAsync();
        }

        // =========================================================
        // ADD
        // إضافة Attachment جديد إلى قاعدة البيانات
        // =========================================================
        public async Task<GigAttachments> AddAsync(GigAttachments attachment)
        {
            // إضافة الـ Attachment الجديد إلى جدول Attachments
            await _context.Attachments.AddAsync(attachment);

            // حفظ التغييرات بشكل فعلي في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع الـ Attachment الذي تمت إضافته
            return attachment;
        }

        // =========================================================
        // UPDATE
        // تعديل Attachment موجود
        // =========================================================
        public async Task<GigAttachments> UpdateAsync(GigAttachments attachment)
        {
            // تحديد أن هذا الـ Attachment تم تعديله
            _context.Attachments.Update(attachment);

            // حفظ التعديل في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع الـ Attachment بعد التعديل
            return attachment;
        }

        // =========================================================
        // DELETE
        // حذف Attachment باستخدام AttachmentId
        // =========================================================
        public async Task<bool> DeleteAsync(int attachmentId)
        {
            // البحث عن الـ Attachment المطلوب حذفه
            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId);

            // إذا لم يتم العثور على Attachment
            if (attachment == null)
                return false; // إرجاع false يعني أن الحذف لم يحدث

            // حذف الـ Attachment من جدول Attachments
            _context.Attachments.Remove(attachment);

            // حفظ عملية الحذف في قاعدة البيانات
            await _context.SaveChangesAsync();

            // إرجاع true يعني أن عملية الحذف تمت بنجاح
            return true;
        }
    }
}