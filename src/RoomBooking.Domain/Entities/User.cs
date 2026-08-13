// Entities/User.cs
namespace RoomBooking.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public DateTime CreatedAt { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
