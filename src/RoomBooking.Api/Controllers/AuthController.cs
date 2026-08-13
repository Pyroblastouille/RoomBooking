using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Interfaces;
using RoomBooking.Application.DTOs.Auth;
using RoomBooking.Application.Interfaces;

namespace RoomBooking.Api.Controllers;

/// <summary>
/// Gère l'inscription et la connexion des utilisateurs.
/// Endpoint : /api/auth
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IHttpResponseHandler _responseHandler;

    public AuthController(IAuthService authService, IHttpResponseHandler responseHandler)
    {
        _authService = authService;
        _responseHandler = responseHandler;
    }

    /// <summary>
    /// POST /api/auth/register
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (!result.Success || result.Data is null)
            return _responseHandler.HandleFailure(result, result.ErrorCode ?? 400);

        return _responseHandler.HandleSuccess(result.Data);
    }

    /// <summary>
    /// POST /api/auth/login
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.Success || result.Data is null)
            return _responseHandler.HandleFailure(result, result.ErrorCode ?? 401);

        return _responseHandler.HandleSuccess(result.Data);
    }
}
