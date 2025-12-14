using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record UserForAuthDto
    {
        [Required(ErrorMessage ="User name is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]

        public string? Password { get; set; }
    }
}
