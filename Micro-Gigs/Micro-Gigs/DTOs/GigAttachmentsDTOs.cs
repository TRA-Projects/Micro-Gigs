using System;
using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    // البيانات التي يتم إرسالها عند رفع مرفق جديد
    public class GigAttachmentsInputDTO
    {
        [Required]
        public Guid GigId { get; set; }          // system generated/linked — linked to Gigs

        [Required]
        public string FileUrl { get; set; }      // system generated — path or URL after upload
    }

    // البيانات التي تظهر عند استعراض المرفق
    public class GigAttachmentsOutputDTO
    {
        public Guid AttachmentId { get; set; }   // system generated — Primary Key
        public Guid GigId { get; set; }          // system generated
        public string FileUrl { get; set; }      // system generated
        public Guid UploadedBy { get; set; }     // system generated — User ID from token
    }
}
