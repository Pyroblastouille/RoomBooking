using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTOs.Rooms;
using RoomBooking.Application.Common;
using RoomBooking.Application.Interfaces;
using RoomBooking.Api.Interfaces;

namespace RoomBooking.Api.Controllers;

/// <summary>
/// Controller pour la gestion des Rooms
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IHttpResponseHandler _responseHandler;

    public RoomsController(IRoomService roomService, IHttpResponseHandler responseHandler)
    {
        _roomService = roomService;
        _responseHandler = responseHandler;
    }

    /// <summary>
    /// GET: api/rooms
    /// Récupère toutes les rooms
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var rooms = await _roomService.GetAllAsync();

        return _responseHandler.HandleSuccess(rooms);
    }

    /// <summary>
    /// GET: api/rooms/{id}
    /// Récupère une room par ID
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
            var room = await _roomService.GetByIdAsync(id);

            if (room is null)
            {
                var failResult = ServiceResult<RoomDto>.Fail($"Room with ID {id} does not exist.");
                return _responseHandler.HandleFailure(failResult);
            }

            return _responseHandler.HandleSuccess(room);
    }

    /// <summary>
    /// POST: api/rooms
    /// Crée une nouvelle room
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoomDto createRoomDto)
    {
        var created = await _roomService.CreateAsync(createRoomDto);
        if(!created)
        {
            var failMessage = ServiceResult<RoomDto>.Fail("Room could not be created");
            return _responseHandler.HandleFailure(failMessage);
        }
        return _responseHandler.HandleSuccess(created);
    }

    /// <summary>
    /// DELETE: api/rooms/{id}
    /// Supprime une room
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var deleted = await _roomService.DeleteAsync(id);
        if (!deleted)
        {
            var failResult = ServiceResult<RoomDto>.Fail($"Room with ID {id} could not be deleted or does not exists");
            return _responseHandler.HandleFailure(failResult);
        }
        return _responseHandler.HandleSuccess(true);

    }
}
