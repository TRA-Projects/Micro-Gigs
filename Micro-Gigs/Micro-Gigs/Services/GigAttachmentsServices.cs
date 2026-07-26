using Micro_Gigs.Models;
// للوصول إلى Model: GigAttachments

using Micro_Gigs.DTOs;
// للوصول إلى DTO:
// GigAttachmentsInputDTO

using Micro_Gigs.Repositories.Interfaces;
// للوصول إلى Interface:
// IGigAttachmentsRepository


namespace Micro_Gigs.Services
{
    //// =========================================================
    //// GIG ATTACHMENTS SERVICE
    //// Service مسؤول عن العمليات الخاصة بـ GigAttachments
    //// =========================================================

    public class GigAttachmentsServices
    {
        //    // =========================================================
        //    // REPOSITORY
        //    // إنشاء متغير للوصول إلى Repository
        //    // =========================================================

        private readonly IGigAttachmentsRepository _repository;


        //    // =========================================================
        //    // CONSTRUCTOR
        //    // استقبال Repository عن طريق Dependency Injection
        //    // =========================================================

        public GigAttachmentsServices(
            IGigAttachmentsRepository repository)
        {
            // تخزين الـ Repository داخل المتغير _repository
            _repository = repository;
        }


        //    // =========================================================
        //    // CREATE ATTACHMENT
        //    // إنشاء Attachment جديد
        //    // =========================================================

        public async Task<GigAttachments> CreateAttachment(
            GigAttachmentsInputDTO input,
            int userId)
        {
            //        // =====================================================
            //        // CREATE NEW ATTACHMENT
            //        // إنشاء Object جديد من Model: GigAttachments
            //        // =====================================================

            var attachment = new GigAttachments
            {
                //            // =================================================
                //            // GIG ID
                //            // أخذ GigId من الـ DTO
                //            // =================================================

                GigId = input.GigId,


                //            // =================================================
                //            // FILE URL
                //            // أخذ رابط أو مسار الملف من الـ DTO
                //            // =================================================

                FileUrl = input.FileUrl,


                //            // =================================================
                //            // USER ID
                //            // تحديد المستخدم الذي قام برفع الملف
                //            // =================================================

                UserID = userId
            };


            //        // =====================================================
            //        // ADD ATTACHMENT
            //        // إضافة الـ Attachment إلى قاعدة البيانات
            //        // AddAsync يقوم أيضًا بحفظ البيانات
            //        // =====================================================

            return await _repository.AddAsync(attachment);
        }
    }
}