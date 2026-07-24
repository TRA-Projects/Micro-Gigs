using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    public class CreateGigDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Budget is required")]
        [Range(0.01, 999999.99)]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int GigCategoryId { get; set; }
    }
    public class GigResponseDto
    {
        public int GigId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
