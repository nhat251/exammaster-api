using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class UserResponse
    {
        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Email { get; set; }
    }
}
