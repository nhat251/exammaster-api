using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class AuthResponse
    {
        public string? AccessToken { get; set; }
        public UserRefreshToken? UserRefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
