using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Streetcode.BLL.Interfaces.URL;

namespace Streetcode.BLL.Services.URL
{
    public class URLGenerator : IURLGenerator
    {
        private LinkGenerator _linkGenerator;
        public URLGenerator(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public string Url(string action, string controller, object values, HttpContext httpContext)
        {
            string url = _linkGenerator.GetPathByAction(action, controller, values) !;
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{url}";
        }
    }
}
