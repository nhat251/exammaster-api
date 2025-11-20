using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impls
{
    public class CookieService : ICookieService
    {
        public void SetCookie(HttpResponse response, string key, string value, DateTime expiredAt)
        {
            response.Cookies.Append(key, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expiredAt
            });
        }
    }
}
