using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InvalidToken : IEntity<Guid>   
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = null!;
        public DateTime RevokedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiredAt { get; set; }

    }
}
