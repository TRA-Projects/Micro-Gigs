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

        public string ProposalText { get; set; }


        [Column(TypeName = "decimal(18,2)]")]
        public decimal ProposedPrice { get; set; }

        public DateTime ApplicationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // خصائص التنقل (Navigation Properties)
        [ForeignKey("GigId")]
        public virtual Gigs Gig { get; set; }

        [ForeignKey("FreelancerId")]
        public virtual Users Freelancer { get; set; }
    }
}