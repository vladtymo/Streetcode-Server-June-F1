using System.Net;
using System.Resources;
using System.Threading.Tasks;

namespace Streetcode.WebApi.Resources
{
    public class CustomErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomErrorHandlingMiddleware> _logger;
        private static ResourceManager _resourceManager = new ResourceManager("Streetcode.WebApi.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);

        public CustomErrorHandlingMiddleware(RequestDelegate next, ILogger<CustomErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorizedAsync(context);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                await HandleBadRequestAsync(context);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                await HandleNotFoundAsync(context);
            }
            else if (context.Response.StatusCode >= 500)
            {
                await HandleServerErrorAsync(context);
            }
        }

        private Task HandleUnauthorizedAsync(HttpContext context)
        {
            return WriteResponseAsync(context, "401", HttpStatusCode.Unauthorized);
        }

        private Task HandleBadRequestAsync(HttpContext context)
        {
            return WriteResponseAsync(context, "400", HttpStatusCode.BadRequest);
        }

        private Task HandleNotFoundAsync(HttpContext context)
        {
            return WriteResponseAsync(context, "404", HttpStatusCode.NotFound);
        }

        private Task HandleServerErrorAsync(HttpContext context)
        {
            return WriteResponseAsync(context, "500", HttpStatusCode.InternalServerError);
        }

        private Task WriteResponseAsync(HttpContext context, string errorCode, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            string errorMessage = _resourceManager.GetString(errorCode) ?? "An unknown error occurred.";
            context.Response.ContentLength = errorMessage.Length;
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = errorMessage
            }.ToString());
        }
    }
}
