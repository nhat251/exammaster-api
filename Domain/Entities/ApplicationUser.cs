using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public string? FullName { get; set; }    
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public string? AvatarUrl { get; set; } 
        public int Balance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
