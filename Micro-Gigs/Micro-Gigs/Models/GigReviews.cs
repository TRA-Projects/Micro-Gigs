using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("GigReviews")]
    public class GigReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // قاعدة البيانات تولد المعرف
        public Guid ReviewId { get; set; } = Guid.NewGuid();   // system generated — Primary Key

        [Required]
        public Guid AssignmentId { get; set; }                 // system generated — linked to GigAssignments (Foreign Key)

        [Required]
        public Guid ReviewerId { get; set; }                   // system generated — set from authenticated user (Foreign Key)

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }                        // user input — numeric value (1 to 5)

        public string? Comment { get; set; }                   // user input — optional text (NVARCHAR(MAX))
    }
}

