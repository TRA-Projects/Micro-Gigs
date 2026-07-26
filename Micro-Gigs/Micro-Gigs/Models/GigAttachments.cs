using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    // اسم الجدول في قاعدة البيانات مطابق تماماً للـ ERD
    [Table("GIG_ATTACHMENTS")]
    public class GigAttachments
    {
        // =========================================================
        // PRIMARY KEY
        // =========================================================
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; } // Primary Key (PK)

        // =========================================================
        // ATTRIBUTES (الخصائص العادية)
        // =========================================================
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty; // FileName

        [Required]
        public string FileUrl { get; set; } = string.Empty; // FileUrl

        public DateTime UploadDate { get; set; } = DateTime.UtcNow; // UploadDate

        // =========================================================
        // FOREIGN KEYS & NAVIGATION PROPERTIES (المفاتيح والعلاقات)
        // =========================================================

        // 1. Gig Relationship (GIGId - FK)
        [Required]
        public int GigId { get; set; }

        [ForeignKey(nameof(GigId))]
        public virtual Gigs? Gig { get; set; }

        // 2. User Relationship (UploadedById - FK)
        [Required]
        public int UploadedById { get; set; }

        [ForeignKey(nameof(UploadedById))]
        public virtual Users? UploadedBy { get; set; }
    }
}