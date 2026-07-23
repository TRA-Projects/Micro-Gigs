using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string UserType { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;


        [Required]
        public string PasswordHash{ get; set; } = string.Empty;
        // Navigation Properties------------------------------------------------------

        // User as Client

        [InverseProperty("Client")]
        public virtual List<Gigs> PostedGigs { get; set; } = new List<Gigs>();

        // User as Freelancer

        [InverseProperty("Freelancer")]

        public virtual List<GigApplications> GigApplications { get; set; } = new List<GigApplications>();

        [InverseProperty("Freelancer")]
        public virtual List<GigAssignments> FreelancerAssignments { get; set; } = new List<GigAssignments>();

        // Reviews written by this user
        [InverseProperty("Reviewer")]

        public virtual List<GigReviews> ReviewsGiven { get; set; } = new List<GigReviews>();

        // Uploaded attachments
        [InverseProperty("UploadedBy")]

        public virtual List<GigAttachments> Uploads { get; set; } = new List<GigAttachments>();
    }
}
