using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICookieService
    {
        public void SetCookie(HttpResponse response, string key, string value, DateTime expiredAt);

    }
}
