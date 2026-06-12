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
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IHttpResponseHandler _responseHandler;

    public BookingsController(IBookingService bookingService, IHttpResponseHandler responseHandler)
    {
        _bookingService = bookingService;
        _responseHandler = responseHandler;
    }


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
        var success = await _bookingService.DeleteAsync(id);

        if (!success)
        {
            var failResult = ServiceResult<object>.Fail($"Booking with ID {id} does not exist.");
            return _responseHandler.HandleFailure(failResult);
        }

        return _responseHandler.HandleSuccess(new { message = "Booking deleted successfully." });
    }
}