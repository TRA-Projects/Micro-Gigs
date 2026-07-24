using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "CategoryName is required")]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

    }
    public class CategoryResponseDto
    {
        public int GigCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
