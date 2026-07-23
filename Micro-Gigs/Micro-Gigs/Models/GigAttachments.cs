using System.ComponentModel.DataAnnotations;
// يحتوي على Data Annotations مثل:
// [Key] و [Required]

using System.ComponentModel.DataAnnotations.Schema;
// يحتوي على:
// [Table] و [DatabaseGenerated] و [ForeignKey]

namespace Micro_Gigs.Models
{
    // =========================================================
    // TABLE NAME
    // تحديد اسم الجدول في قاعدة البيانات
    // =========================================================

    [Table("GigAttachments")]
    // سيتم إنشاء جدول في قاعدة البيانات باسم GigAttachments

    public class GigAttachments
    {
        // =========================================================
        // PRIMARY KEY
        // المفتاح الأساسي للملف
        // =========================================================

        [Key]
        // تحديد أن AttachmentId هو Primary Key

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // قاعدة البيانات تقوم تلقائياً بتوليد رقم AttachmentId
        // مثال:
        // 1, 2, 3, 4 ...

        public int AttachmentId { get; set; }
        // رقم الملف
        // System Generated
        // Primary Key


        // =========================================================
        // GIG ID
        // ربط الملف مع الـ Gig
        // =========================================================

        [Required]
        // الحقل إجباري ولا يمكن أن يكون فارغاً

        public int GigId { get; set; }
        // رقم الـ Gig المرتبط بهذا الملف
        // Foreign Key
        // يحدد لأي Gig ينتمي هذا الملف


        // =========================================================
        // FILE URL
        // رابط أو مسار الملف
        // =========================================================

        [Required]
        // يجب أن يحتوي الحقل على قيمة
        // لا يمكن أن يكون فارغاً

        public string FileUrl { get; set; }
        // مسار أو رابط الملف
        // مثال:
        // "uploads/files/project.pdf"
        //
        // أو:
        // "https://example.com/files/project.pdf"
        //
        // System Generated
        // يتم تخزين رابط أو مسار الملف


        // =========================================================
        // USER ID
        // المستخدم الذي قام برفع الملف
        // =========================================================

        [Required]
        // الحقل إجباري

        public int UserID { get; set; }
        // رقم المستخدم الذي قام برفع الـ Attachment
        // Foreign Key
        // User ID of the uploader
    }
}