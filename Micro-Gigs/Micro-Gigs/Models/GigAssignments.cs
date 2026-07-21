using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    public class GigAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }

        [ForeignKey(nameof(Gigs))]
        public int GigId { get; set; }
       
        public int freelancerId { get; set; }
        public decimal AgreedPrice { get; set; }
        public  string Status { get; set; }
    }
}
