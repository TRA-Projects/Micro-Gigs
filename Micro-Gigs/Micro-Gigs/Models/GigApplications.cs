using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    public class GigApplications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }

        [Required]
        public int GigId { get; set; }

        [Required]
        public int FreelancerId { get; set; }

        [MaxLength(2000)]
        public string ProposalText { get; set; } = string.Empty; // إعطاؤها قيمة افتراضية

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProposedPrice { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // (Navigation Properties) - إضافة required أو علامة الاستفهام ?
        [ForeignKey("GigId")]
        public virtual Gigs Gig { get; set; } = null!;

        [ForeignKey("FreelancerId")]
        public virtual Users Freelancer { get; set; } = null!;
    }
}