// Entities/Booking.cs
namespace RoomBooking.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
