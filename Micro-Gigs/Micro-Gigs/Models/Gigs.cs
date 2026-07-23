using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("Gigs")]
    public class Gigs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GigId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Budget { get; set; }

        public DateTime DueDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Open"; // Open, Assigned, Completed, Cancelled

        public DateTime PostedDate { get; set; } = DateTime.Now;

        [Required]
        public int ClientId { get; set; } // Foreign Key to Users table

        [ForeignKey("ClientId")]
        public virtual Users Client { get; set; }//  // Navigation Properties

        [Required]
        public int GigCategoryId { get; set; } // Foreign Key to GigCategories table

        [ForeignKey("GigCategoryId")]
        public virtual GigCategories GigCategory { get; set; } // Navigation Property

        public virtual GigAssignments GigAssignment { get; set; } // Navigation Property
        public virtual List<GigApplications> GigApplications { get; set; } = new List<GigApplications>(); // Navigation Property
        public virtual List<GigAttachments> GigAttachments { get; set; } = new List<GigAttachments>(); // Navigation Property

    }
}
