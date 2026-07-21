using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    // المدخلات: البيانات التي يرسلها المستخدم عند رفع مرفق جديد
    public class GigAttachmentsInputDTOs
    {
        //[Required]
        //public Guid GigId { get; set; }          // system generated — linked to the specific Gig

        //[Required]
        //public string FileUrl { get; set; }      // system generated — the path/URL after uploading the file
    }

    // المخرجات: البيانات التي تظهر عند استعراض المرفقات
    public class GigAttachmentsOutputDTOs
    {
        //public Guid AttachmentId { get; set; }   // system generated — Primary Key

        //public Guid GigId { get; set; }          // system generated

        //public string FileUrl { get; set; }      // system generated — the link to the file

        //public Guid UserID { get; set; }     // system generated — ID of the user who performed the upload
    }
}
