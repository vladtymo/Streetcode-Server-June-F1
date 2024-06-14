using System.Resources;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.WebApi.Resources;

namespace Streetcode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    private static ResourceManager _resourceManager = new ResourceManager("YourNamespace.ErrorMessages", typeof(ErrorMessages).Assembly);


    protected IMediator Mediator => _mediator ??=
        HttpContext.RequestServices.GetService<IMediator>()!;

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if(result is NullResult<T>)
            {
                return Ok(result.Value);
            }

            return (result.Value is null) ?
                NotFound("Found result matching null") : Ok(result.Value);
        }

        var errorMessage = "An unknown error occurred.";
        if (result.Errors.Count > 0)
        {
            var errorCode = result.Errors[0].Message; // Assuming the error code is stored in the Message property
            errorMessage = _resourceManager.GetString(errorCode) ?? errorMessage;
        }

       // return BadRequest(new { Error = errorMessage });

        return BadRequest(result.Reasons);
    }
}