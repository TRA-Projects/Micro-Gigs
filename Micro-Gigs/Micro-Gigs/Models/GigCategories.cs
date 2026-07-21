using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("Gig_Categories")]
    public class GigCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GigCategoryId { get; set; }

        [Required] 
        public string CategoryName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
