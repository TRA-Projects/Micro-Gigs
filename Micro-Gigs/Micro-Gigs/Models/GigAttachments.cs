using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    
    [Table("GigAttachments")]
    public class GigAttachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int AttachmentId { get; set; } // system generated — Primary Key

        [Required]
        public int GigId { get; set; }                       // system generated — linked to Gigs (Foreign Key)

        [Required]
        public string FileUrl { get; set; }                 // system generated — path or URL to the file

        [Required]
        public int UserID { get; set; }                    // system generated — User ID of the uploader (Foreign Key)

        //------------------------------------------------------------------------------------------------------------------------------//
        // Navigation properties (Relationships)

        [ForeignKey("GigId")]
        public Gigs Gig { get; set; }  // Reference to the specific gig

        [ForeignKey("UserID")]
        public Users Uploader { get; set; }  // Reference to the user who uploaded the file
    }
}
