using System;
using System.ComponentModel.DataAnnotations;

namespace Micro_Gigs.DTOs
{
    public class GigApplicationDto
    {
        public int ApplicationId { get; set; }
        public int GigId { get; set; }
        public string GigTitle { get; set; } = string.Empty;
        public int FreelancerId { get; set; }
        public string FreelancerName { get; set; } = string.Empty;
        public string ProposalText { get; set; } = string.Empty;
        public decimal ProposedPrice { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateGigApplicationDto
    {
        [Required(ErrorMessage = "GigId is required")]
        public int GigId { get; set; }

        [Required(ErrorMessage = "FreelancerId is required")]
        public int FreelancerId { get; set; }

        [Required(ErrorMessage = "ProposalText is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Proposal text must be between 10 and 2000 characters.")]
        public string ProposalText { get; set; } = string.Empty;

        [Required(ErrorMessage = "ProposedPrice is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Please enter a valid proposed price between 0.01 and 999999.99")]
        public decimal ProposedPrice { get; set; }
    }
}