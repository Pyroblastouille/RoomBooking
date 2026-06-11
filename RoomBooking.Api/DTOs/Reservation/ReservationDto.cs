namespace RoomBooking.Api.DTOs.Reservations;

public class ReservationDto
{
    public int Id { get; set; }

    public string Title { get; set; }= string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }

    public string RoomName { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
}
