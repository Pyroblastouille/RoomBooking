namespace RoomBooking.Api.DTOs.Rooms;

public class RoomDto
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public int Capacity {get; set;}
    public string? Description {get;set;}
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}