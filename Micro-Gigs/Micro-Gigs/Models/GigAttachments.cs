using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    
    [Table("GigAttachments")]
    public class GigAttachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AttachmentId { get; set; } = Guid.NewGuid(); // system generated — Primary Key

        [Required]
        public Guid GigId { get; set; }                         // system generated — linked to Gigs (Foreign Key)

        [Required]
        public string FileUrl { get; set; }                     // system generated — path or URL to the file

        [Required]
        public Guid UploadedBy { get; set; }                    // system generated — User ID of the uploader (Foreign Key)
    }
}
