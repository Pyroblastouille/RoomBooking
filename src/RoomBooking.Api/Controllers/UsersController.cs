using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTOs.Users;
using RoomBooking.Application.Common;
using RoomBooking.Application.Interfaces;
using RoomBooking.Api.Interfaces;
using RoomBooking.Application.DTOs.Rooms;

namespace RoomBooking.Api.Controllers;

/// <summary>
/// Controller pour la gestion des Users (réservé aux administrateurs).
/// L'inscription libre-service se fait via /api/auth/register.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpResponseHandler _responseHandler;

    public UsersController(IUserService userService, IHttpResponseHandler responseHandler)
    {
        _userService = userService;
        _responseHandler = responseHandler;
    }

    /// <summary>
    /// GET: api/users
    /// Récupère tous les users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();

        return _responseHandler.HandleSuccess(users);
    }

    /// <summary>
    /// GET: api/users/{id}
    /// Récupère un user par ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
            var user = await _userService.GetByIdAsync(id);

            if (user is null)
            {
                var failResult = ServiceResult<UserDto>.Fail($"User with ID {id} does not exist.");
                return _responseHandler.HandleFailure(failResult);
            }

            return _responseHandler.HandleSuccess(user);
    }

    /// <summary>
    /// POST: api/users
    /// Crée un nouvel user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserDto createUserDto)
    {
        var created = await _userService.CreateAsync(createUserDto);
        if(!created)
        {
            var failMessage = ServiceResult<UserDto>.Fail("User could not be created");
            return _responseHandler.HandleFailure(failMessage);
        }
        return _responseHandler.HandleSuccess(created);
    }

    /// <summary>
    /// DELETE: api/users/{id}
    /// Supprime un user
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result)
        {
            var failResult = ServiceResult<UserDto>.Fail($"User with ID {id} could not be deleted or does not exists");
            return _responseHandler.HandleFailure(failResult);
        }
        return _responseHandler.HandleSuccess(result);

    }

    /// <summary>
    /// POST: api/users
    /// Crée un nouvel user
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] UpdateUserDto updateUserDto, [FromRoute] int id) {
        var updated = await _userService.UpdateAsync(id,updateUserDto);
        if (!updated) {
            var failMessage = ServiceResult<UserDto>.Fail("User could not be updated");
            return _responseHandler.HandleFailure(failMessage);
        }
        return _responseHandler.HandleSuccess(updated);
    }
}
