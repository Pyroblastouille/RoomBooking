using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.DTOs.Bookings;

public class BookingDto
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

    public static BookingDto FromEntity(Booking booking) {
        if(booking.User is null || booking.Room is null) {
            throw new ApplicationException("Exception transmise sans utilisateur ou pièce attitré");
        }
        return new(){
            Id = booking.Id,
            Title = booking.Title,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            CreatedAt = booking.CreatedAt,
            RoomId = booking.RoomId,
            UserId = booking.UserId,
            RoomName = booking.Room.Name,
            UserFullName = $"{booking.User.FirstName} {booking.User.LastName}"
        };
    }
}
