using Microsoft.AspNetCore.Http;

namespace Streetcode.BLL.Services.CookieService.Interfaces
{
    public interface ICookieService
    {
        Task ClearCookiesAsync(HttpContext httpContext);
    }
}
