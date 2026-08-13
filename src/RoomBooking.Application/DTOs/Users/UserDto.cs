using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.DTOs.Users;

public class UserDto
{
    public int Id {get; set;}
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
}