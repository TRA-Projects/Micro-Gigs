using System;

using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    // البيانات التي يدخلها العميل لتقييم المستقل
    public class GigReviewsInputDTO
    {
        [Required]
        public Guid AssignmentId { get; set; }   // system generated/linked — linked to Assignment

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }          // user input — numeric value (1 to 5)

        public string? Comment { get; set; }     // user input — optional feedback text
    }

    // البيانات التي تظهر عند عرض التقييم
    public class GigReviewsOutputDTO
    {
        public Guid ReviewId { get; set; }       // system generated — Primary Key
        public Guid AssignmentId { get; set; }   // system generated
        public Guid ReviewerId { get; set; }     // system generated — Client ID from token
        public int Rating { get; set; }          // user input
        public string? Comment { get; set; }     // user input
    }
}
