using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Streetcode.WebApi.Extensions;

namespace Streetcode.WebApi.Middlewares;

public sealed class GenericExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GenericExceptionHandlerMiddleware> _logger;

    public GenericExceptionHandlerMiddleware(RequestDelegate next, ILogger<GenericExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError($"Validation error: {ex}");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var validationFailureResponse = new
        {
            Errors = exception.Errors.Select(x => new
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            }).ToList()
        };

        return context.Response.WriteAsync(SerializeJson(validationFailureResponse));
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorDetails = exception.GetErrorDetailsAndStatusCode();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)errorDetails.StatusCode;

        return context.Response.WriteAsync(SerializeJson(errorDetails));
    }

    private static string SerializeJson<T>(T data)
    {
        var camelCasePropertiesSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        camelCasePropertiesSetting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        return JsonConvert.SerializeObject(data, camelCasePropertiesSetting);
    }
}
