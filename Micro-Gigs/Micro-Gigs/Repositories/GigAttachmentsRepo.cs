using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Micro_Gigs.Models;

namespace Micro_Gigs.Repositories
{
    public class GigAttachmentsRepo
    {
        private readonly MicroGigsContext _context;

        public GigAttachmentsRepo(MicroGigsContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET ALL
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetAllAsync()
        {
            return await _context.Attachments
                .AsNoTracking()
                .ToListAsync();
        }

        // =========================================================
        // GET BY ID
        // =========================================================
        public async Task<GigAttachments?> GetByIdAsync(int attachmentId)
        {
            return await _context.Attachments
                .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId);
        }

        // =========================================================
        // GET BY GIG ID
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetByGigIdAsync(int gigId)
        {
            return await _context.Attachments
                .Where(a => a.GigId == gigId)
                .AsNoTracking()
                .ToListAsync();
        }

        // =========================================================
        // GET BY USER ID
        // تم تصحيح اسم الحقل إلى UserID ليتطابق مع Model GigAttachments
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetByUserIdAsync(int userId)
        {
            return await _context.Attachments
                .Where(a => a.UploadedBy == userId) 
                .AsNoTracking()
                .ToListAsync();
        }

        // =========================================================
        // ADD
        // =========================================================
        public async Task<GigAttachments> AddAsync(GigAttachments attachment)
        {
            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        // =========================================================
        // UPDATE
        // =========================================================
        public async Task<GigAttachments> UpdateAsync(GigAttachments attachment)
        {
            _context.Attachments.Update(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        // =========================================================
        // DELETE
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