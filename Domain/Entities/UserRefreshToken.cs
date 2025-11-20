using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserRefreshToken : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RefreshTokenString { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReplacedByToken { get; set; }

        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;

    }
}
