using Microsoft.EntityFrameworkCore;       // لاستخدام ToListAsync و FirstOrDefaultAsync
using Micro_Gigs.Models;                   // للوصول إلى Model: GigAttachments

namespace Micro_Gigs.Repositories.Implementations
{
    // Repository مسؤول عن التعامل مع بيانات GigAttachments في قاعدة البيانات
    public class GigAttachmentsRepo
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
            return await _context.Attachments
                .ToListAsync();
        }

        // =========================================================
        // GET BY ID
        // جلب Attachment واحد باستخدام AttachmentId
        // =========================================================
        public async Task<GigAttachments?> GetByIdAsync(int attachmentId)
        {
            return await _context.Attachments
                .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId);
        }

        // =========================================================
        // GET BY GIG ID
        // جلب جميع الملفات المرتبطة بـ Gig معين
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetByGigIdAsync(int gigId)
        {
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
            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        // =========================================================
        // UPDATE
        // تعديل Attachment موجود
        // =========================================================
        public async Task<GigAttachments> UpdateAsync(GigAttachments attachment)
        {
            _context.Attachments.Update(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        // =========================================================
        // DELETE
        // حذف Attachment باستخدام AttachmentId
        // =========================================================
        public async Task<bool> DeleteAsync(int attachmentId)
        {
            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId);

            if (attachment == null)
                return false;

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}