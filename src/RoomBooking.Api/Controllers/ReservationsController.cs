using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.DTOs.Bookings;
using RoomBooking.Application.Common;
using RoomBooking.Application.Interfaces;
using RoomBooking.Api.Interfaces;

namespace RoomBooking.Api.Controllers;

/// <summary>
/// Gère les opérations CRUD sur les réservations
/// Endpoint : /api/bookings
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IHttpResponseHandler _responseHandler;

    public BookingsController(IBookingService bookingService, IHttpResponseHandler responseHandler)
    {
        _bookingService = bookingService;
        _responseHandler = responseHandler;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsAdmin => User.IsInRole("Admin");


/// <summary>
/// Crée une nouvelle réservation.
/// POST /api/bookings
/// </summary>
/// <param name="dto">Données de la réservation à créer</param>
/// <returns>201 Created si succès, 400/404/409 sinon</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBookingDto dto)
    {
        if (!IsAdmin)
            dto.UserId = CurrentUserId;

        var result = await _bookingService.CreateAsync(dto);

        if (!result.Success || result.Data is null)
            return _responseHandler.HandleFailure(result);

        var location = $"api/bookings/{result.Data.Id}";
        return _responseHandler.HandleSuccess(result.Data, location);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {   
        //Envoit au service
        var bookings = await _bookingService.GetAllAsync();

        //Renvoit la réponse du service
        return _responseHandler.HandleSuccess(bookings);
    }
  [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        //try to get
        var booking = await _bookingService.GetByIdAsync(id);

        //handle failure
        if (booking is null)
        {
            var failResult = ServiceResult<BookingDto>.Fail($"Booking with ID {id} does not exist.");
            return _responseHandler.HandleFailure(failResult);
        }

        //handle success
        return _responseHandler.HandleSuccess(booking);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateBookingDto dto)
    {
        var existing = await _bookingService.GetByIdAsync(id);
        if (existing is null)
        {
            var notFound = ServiceResult<BookingDto>.Fail($"Booking with ID {id} does not exist.", 404);
            return _responseHandler.HandleFailure(notFound, 404);
        }

        if (!IsAdmin && existing.UserId != CurrentUserId)
        {
            var forbidden = ServiceResult<BookingDto>.Fail("You can only modify your own bookings.", 403);
            return _responseHandler.HandleFailure(forbidden, 403);
        }

        if (!IsAdmin)
            dto.UserId = CurrentUserId;

        //try to update
        var result = await _bookingService.UpdateAsync(id, dto);

        //handle failure
        if (!result.Success)
            return _responseHandler.HandleFailure(result);

        //handle success
        return _responseHandler.HandleSuccess(result.Data);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var existing = await _bookingService.GetByIdAsync(id);
        if (existing is null)
        {
            var notFound = ServiceResult<object>.Fail($"Booking with ID {id} does not exist.", 404);
            return _responseHandler.HandleFailure(notFound, 404);
        }

        if (!IsAdmin && existing.UserId != CurrentUserId)
        {
            var forbidden = ServiceResult<object>.Fail("You can only delete your own bookings.", 403);
            return _responseHandler.HandleFailure(forbidden, 403);
        }

        var success = await _bookingService.DeleteAsync(id);

        if (!success)
        {
            var failResult = ServiceResult<object>.Fail($"Booking with ID {id} does not exist.");
            return _responseHandler.HandleFailure(failResult);
        }

        return _responseHandler.HandleSuccess(new { message = "Booking deleted successfully." });
    }
}