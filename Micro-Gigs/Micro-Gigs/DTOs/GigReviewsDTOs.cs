using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    //// المدخلات: البيانات التي يرسلها العميل لتقييم مستقل بعد انتهاء المهمة
    //public class GigReviewsInputDTOs
    //{
    //    [Required]
    //    public Guid AssignmentId { get; set; } // system generated — the link to the specific completed assignment

    //    [Required]
    //    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    //    public int Rating { get; set; }        // user input — numeric value (1 to 5)

    //    public string? Comment { get; set; }   // user input — optional feedback text
    }

    // المخرجات: البيانات التي يتم عرضها عند قراءة التقييمات
    public class GigReviewsOutputDTOs
    {
        //public Guid ReviewId { get; set; }     // system generated — Primary Key

        //public Guid AssignmentId { get; set; } // system generated

        //public Guid ReviewerId { get; set; }   // system generated — ID of the Client who wrote the review

        //public int Rating { get; set; }        // user input — the rating value

        //public string? Comment { get; set; }   // user input — the feedback text
    }

