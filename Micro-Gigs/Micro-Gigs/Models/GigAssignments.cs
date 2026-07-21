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

    
        
        [Required]
        public int freelancerId { get; set; }
        public decimal AgreedPrice { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime? CompletionDate { get; set; }
        //
        [Required]
        [MaxLength(20)]
        public  string Status { get; set; }//InProgress, Submitted, Approved, Rejected, Completed
    }
}
