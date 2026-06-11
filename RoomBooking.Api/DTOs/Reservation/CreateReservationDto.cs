using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Api.DTOs.Reservations;

// Ce qu'on REÇOIT pour créer une réservation
public class CreateReservationDto
{

    [Required(ErrorMessage = "Title required")]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    

    [Required(ErrorMessage = "Start Time required.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End Time required.")]
    public DateTime EndTime { get; set; }

    [Required(ErrorMessage = "Room required")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "User required.")]
    public int UserId { get; set; }
}