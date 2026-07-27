using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("GIG_ASSIGNMENTS")]
    public class GigAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; } // system generated — Primary Key

        public int GigId { get; set; }

        [Required]
        public int freelancerId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AgreedPrice { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // InProgress, Submitted, Approved, Rejected, Completed


        [ForeignKey(nameof(GigId))]
        public virtual Gigs? Gig { get; set; }

        [ForeignKey(nameof(freelancerId))]
        public virtual Users? Freelancer { get; set; }
    }
}