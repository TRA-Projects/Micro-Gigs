using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("GigReviews")]
    public class GigReviews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // قاعدة البيانات تولد المعرف
        public int ReviewId { get; set; }   // system generated — Primary Key

        [Required]
        public int AssignmentId { get; set; }                 // system generated — linked to GigAssignments (Foreign Key)

        [Required]
        public int ReviewerId { get; set; }                   // system generated — set from authenticated user (Foreign Key)

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }                        // user input — numeric value (1 to 5)

        public string? Comment { get; set; }                   // user input — optional text (NVARCHAR(MAX))

        //------------------------------------------------------------------------------------------------------------------------------//
        // Navigation properties (العلاقات)

        // Navigation properties (Relationships)

        [ForeignKey("AssignmentId")]
        public virtual GigAssignments Assignment { get; set; }

        [ForeignKey("ReviewerId")]
        public virtual Users Reviewer { get; set; }
    }
}

