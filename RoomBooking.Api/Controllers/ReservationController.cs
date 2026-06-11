using Microsoft.AspNetCore.Mvc;
using RoomBooking.Api.Interfaces;

[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        
    }
}