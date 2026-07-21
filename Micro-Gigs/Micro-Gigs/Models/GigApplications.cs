using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.Models
{
    public class GigApplications
    {
        [Key]
        public Guid ApplicationId { get; set; }
        //
        [Required]
        public Guid GigId { get; set; }

        [Required]
        public Guid FreelancerId { get; set; }

        public string ProposalText { get; set; }
        //
        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // علاقات التنقل (Navigation Properties) الاختيارية
        //[ForeignKey("GigId")]
        //public virtual Gig Gig { get; set; }

        //[ForeignKey("FreelancerId")]
        //public virtual User Freelancer { get; set; }

    }
}
