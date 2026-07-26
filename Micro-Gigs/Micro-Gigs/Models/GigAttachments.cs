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
        [MaxLength(500)]
        public string FileUrl { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? FileName { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // =========================================================
        // FOREIGN KEYS & NAVIGATION PROPERTIES
        // =========================================================

        // 1. Gig Relationship
        [Required]
        public int GigId { get; set; }

        [ForeignKey("GigId")]
        public virtual Gigs Gig { get; set; } = null!;

        // 2. User Relationship
        [Required]
        public int UploadedBy { get; set; }

        [ForeignKey("UploadedBy")]
        public virtual Users UploadedByUser { get; set; } = null!;
    }
}