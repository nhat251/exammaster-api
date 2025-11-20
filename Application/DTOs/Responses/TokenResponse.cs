using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = null!;
        public UserRefreshToken UserRefreshToken { get; set; } = null!;
    }
}
