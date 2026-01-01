using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public class GeneratePayrollRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Company ID is required")]
        public int CompanyId { get; set; }

        [Required]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100")]
        public int Year { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        public int Month { get; set; }
        public string? GeneratedBy { get; set; }
    }

}
