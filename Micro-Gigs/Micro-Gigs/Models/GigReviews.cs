using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    //  اسم الجدول بالكامل كما في الـ ERD
    [Table("GIG_REVIEWS")]
    public class GigReviews
    {
        // =========================================================
        // PRIMARY KEY
        // =========================================================
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        // =========================================================
        // ATTRIBUTES
        // =========================================================
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        //  تاريخ التقييم الموضّح في الـ ERD
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        // =========================================================
        // FOREIGN KEYS & NAVIGATION PROPERTIES
        // =========================================================

        // 1. Assignment Relationship (تم التعديل إلى AssId ليطابق ERD)
        [Required]
        public int AssId { get; set; }

        [ForeignKey(nameof(AssId))]
        public virtual GigAssignments? Assignment { get; set; }

        // 2. Client/User Relationship (تم التعديل إلى ClientId ليطابق ERD)
        [Required]
        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Users? Client { get; set; }
    }
}