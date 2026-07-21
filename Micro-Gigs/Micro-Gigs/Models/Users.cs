using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_Gigs.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }= string.Empty;

        [Required]
        [MaxLength(20)]
        public string UserType { get; set; }= string.Empty;

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties


    }
}
