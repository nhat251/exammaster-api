using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
