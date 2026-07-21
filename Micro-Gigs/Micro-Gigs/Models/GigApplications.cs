using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    public class GigApplications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid ApplicationId { get; set; }

        [Required]
        public Guid GigId { get; set; }

        [Required]
        public Guid FreelancerId { get; set; }

        public string ProposalText { get; set; }

        // أضفنا الحقلين المفقودين هنا ليتوافقا مع الخدمة وقاعدة البيانات
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