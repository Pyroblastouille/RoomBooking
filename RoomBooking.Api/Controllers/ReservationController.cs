using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Interfaces;
using RoomBooking.Api.DTOs.Reservations;
using RoomBooking.Api.Common;

/// <summary>
/// Gère les opérations CRUD sur les réservations
/// Endpoint : /api/reservations
/// </summary>
[ApiController]
[Route("api/reservations")]
public class UserController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IHttpResponseHandler _responseHandler;

    public UserController(IReservationService reservationService, IHttpResponseHandler responseHandler)
    {
        _reservationService = reservationService;
        _responseHandler = responseHandler;
    }


/// <summary>
/// Crée une nouvelle réservation.
/// POST /api/reservations
/// </summary>
/// <param name="dto">Données de la réservation à créer</param>
/// <returns>201 Created si succès, 400/404/409 sinon</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateReservationDto dto)
    {
        var result = await _reservationService.CreateAsync(dto);

        if (!result.Success || result.Data is null)
            return _responseHandler.HandleFailure(result);

        var location = $"api/reservations/{result.Data.Id}";
        return _responseHandler.HandleSuccess(result.Data, location);
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {   
        //Envoit au service
        var reservations = await _reservationService.GetAllAsync();

        //Renvoit la réponse du service
        return _responseHandler.HandleSuccess(reservations);
    }
  [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        //try to get
        var reservation = await _reservationService.GetByIdAsync(id);

        //handle failure
        if (reservation is null)
        {
            var failResult = ServiceResult<ReservationDto>.Fail($"Reservation with ID {id} does not exist.");
            return _responseHandler.HandleFailure(failResult);
        }

        //handle success
        return _responseHandler.HandleSuccess(reservation);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int id,
        [FromBody] UpdateReservationDto dto)
    {
        //try to update
        var result = await _reservationService.UpdateAsync(id, dto);

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
        var success = await _reservationService.DeleteAsync(id);

        if (!success)
        {
            var failResult = ServiceResult<object>.Fail($"Reservation with ID {id} does not exist.");
            return _responseHandler.HandleFailure(failResult);
        }

        return _responseHandler.HandleSuccess(new { message = "Reservation deleted successfully." });
    }
}