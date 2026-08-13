using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Interfaces;
using RoomBooking.Application.Common;

namespace RoomBooking.Api.Common;

public class HttpResponseHandler : ControllerBase, IHttpResponseHandler
{
    public IActionResult HandleSuccess<T>(T data, string? location = null)
    {
        if (!string.IsNullOrEmpty(location))
        {
            return Created(location, new
            {
                isSuccess = true,
                message = "Creation of ressource successful",
                data = data
            });
        }

        return Ok(new
        {
            isSuccess = true,
            message = "Operation successful",
            data = data
        });
    }

    public IActionResult HandleFailure<T>(ServiceResult<T> result,int code = 0)
    {
        if (code < 400 || code >= 500)
            code = DetermineStatusCode(result.ErrorMessage is null ? "" : result.ErrorMessage);
        return StatusCode(code, new
        {
            isSuccess=false,
            message=result.ErrorMessage
        });
    }

    private static int DetermineStatusCode(string message)
    {
        
        // 409 Conflict : réservation en conflit
        if (message.Contains("already reserved", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("conflict", StringComparison.OrdinalIgnoreCase))
        {
            return StatusCodes.Status409Conflict;
        }

        // 404 Not Found : ressource inexistante
        if (message.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("not found", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("not active", StringComparison.OrdinalIgnoreCase))
        {
            return StatusCodes.Status404NotFound;
        }

        // 400 Bad Request : (par défaut)
        return StatusCodes.Status400BadRequest;
    }
}