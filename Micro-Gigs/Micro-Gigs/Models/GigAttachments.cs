using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("GIG_ATTACHMENTS")]
    public class GigAttachments
    {
        // =========================================================
        // PRIMARY KEY
        // =========================================================
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }

        // =========================================================
        // FILE ATTRIBUTES
        // =========================================================
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FileUrl { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        // =========================================================
        // FOREIGN KEYS & NAVIGATION PROPERTIES
        // =========================================================

        // 1. Gig Relationship
        [Required]
        public int GigId { get; set; }

        [ForeignKey(nameof(GigId))]
        public virtual Gigs? Gig { get; set; }

        // 2. User Relationship
        [Required]
        public int UploadedById { get; set; }

        [ForeignKey(nameof(UploadedById))]
        public virtual Users? UploadedBy { get; set; }
    }
}