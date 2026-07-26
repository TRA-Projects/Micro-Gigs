using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("GIG_ASSIGNMENTS")]
    public class GigAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }
        public decimal AgreedPrice { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        [Required]
        [MaxLength(20)]
        public  string Status { get; set; }//InProgress, Submitted, Approved, Rejected, Completed
        //  تمت إضافة رقم الخدمة (GigId) لربط التعيين بالخدمة
        [Required]
        public int GigId { get; set; }
        [Required]
        public int FreelancerId { get; set; }
        // خصائص التنقل  التي تحل الخطأ تماماً(Navigation Properties)
        [ForeignKey("GigId")]
        public virtual Gigs? Gig { get; set; } = null;

        [ForeignKey("FreelancerId")]
        public virtual Users? Freelancer { get; set; } = null;
        public virtual GigReviews? Review { get; set; }
    
    }

}
