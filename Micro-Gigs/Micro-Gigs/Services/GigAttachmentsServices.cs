using Micro_Gigs.Models;                           
using Micro_Gigs.DTOs;                             
using Micro_Gigs.Repositories.Implementations;    
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Micro_Gigs.Services
{
    // =========================================================
    // GIG ATTACHMENTS SERVICE
    // Service مسؤول عن العمليات الخاصة بـ GigAttachments
    // =========================================================

    public class GigAttachmentsServices
    {
        // =========================================================
        // REPOSITORY
        // إنشاء متغير للوصول إلى Repository المباشر
        // =========================================================
        private readonly GigAttachmentsRepo _repository;

        private readonly IWebHostEnvironment _env;

        // =========================================================
        // CONSTRUCTOR
        // استقبال Repository و IWebHostEnvironment عن طريق Dependency Injection
        // =========================================================
        public GigAttachmentsServices(GigAttachmentsRepo repository, IWebHostEnvironment env)
        {
            // تخزين الـ Repository داخل المتغير _repository
            _repository = repository;
            _env = env;
        }

        // =========================================================
        // CREATE ATTACHMENT
        // إنشاء Attachment جديد
        // =========================================================
        public async Task<GigAttachments> CreateAttachment(
            GigAttachmentsInputDTO input,
            int userId)
        {
            // =====================================================
            // CREATE NEW ATTACHMENT
            // إنشاء Object جديد من Model: GigAttachments
            // =====================================================
            var attachment = new GigAttachments
            {
                // =================================================
                // GIG ID
                // أخذ GigId من الـ DTO
                // =================================================
                GigId = input.GigId,

                // =================================================
                // FILE URL
                // أخذ رابط أو مسار الملف من الـ DTO
                // =================================================
                FileUrl = input.FileUrl,

                // =================================================
                // USER ID
                // تحديد المستخدم الذي قام برفع الملف (من الـ Token)
                // =================================================
                UploadedBy = userId
            };

            // =====================================================
            // ADD ATTACHMENT
            // إضافة الـ Attachment إلى قاعدة البيانات
            // AddAsync يقوم أيضًا بحفظ البيانات
            // =====================================================
            return await _repository.AddAsync(attachment);
        }

        // =========================================================
        // CREATE ATTACHMENT FROM IFormFile
        // Saves file to wwwroot/uploads and creates DB record
        // =========================================================
        public async Task<GigAttachmentsOutputDTO> CreateAttachmentFromFile(IFormFile file, int gigId, int userId)
        {
            // ensure uploads folder exists
            var uploadsRoot = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
            if (!Directory.Exists(uploadsRoot)) Directory.CreateDirectory(uploadsRoot);

            var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsRoot, uniqueName);

            // save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // create public URL (relative to app root)
            var relativeUrl = $"/uploads/{uniqueName}";

            var model = new GigAttachments
            {
                GigId = gigId,
                FileUrl = relativeUrl,
                FileName = file.FileName,
                UploadedBy = userId
            };

            var created = await _repository.AddAsync(model);

            return new GigAttachmentsOutputDTO
            {
                AttachmentId = created.AttachmentId,
                GigId = created.GigId,
                FileUrl = created.FileUrl,
                FileName = created.FileName,
                UploadedBy = created.UploadedBy
            };
        }

        // =========================================================
        // READ - ALL / BY ID / BY GIG / BY USER
        // =========================================================
        public async Task<IEnumerable<GigAttachments>> GetAllAttachments()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<GigAttachments?> GetById(int attachmentId)
        {
            return await _repository.GetByIdAsync(attachmentId);
        }

        public async Task<IEnumerable<GigAttachments>> GetByGigId(int gigId)
        {
            return await _repository.GetByGigIdAsync(gigId);
        }

        public async Task<IEnumerable<GigAttachments>> GetByUserId(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        // =========================================================
        // UPDATE
        // Only uploader can update metadata
        // =========================================================
        public async Task<GigAttachments?> UpdateAttachment(int attachmentId, GigAttachmentsInputDTO input, int userId)
        {
            var attachment = await _repository.GetByIdAsync(attachmentId);
            if (attachment == null) return null;
            if (attachment.UploadedBy != userId) return null;

            // update allowed fields (FileUrl only in DTO)
            attachment.FileUrl = input.FileUrl;

            return await _repository.UpdateAsync(attachment);
        }

        // =========================================================
        // DELETE
        // Only uploader can delete
        // =========================================================
        public async Task<bool> DeleteAttachment(int attachmentId, int userId)
        {
            var attachment = await _repository.GetByIdAsync(attachmentId);
            if (attachment == null) return false;
            if (attachment.UploadedBy != userId) return false;

            return await _repository.DeleteAsync(attachmentId);
        }
    }
}