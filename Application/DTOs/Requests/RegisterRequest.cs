using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }

        public string Password { get; set; } = string.Empty;
    }

}
