namespace RoomBooking.Api.Models;

/// <summary>
/// Structure of a Room
/// </summary>
public class Room
{
#region Identifier
    public int Id {get; set;}
    
#endregion


#region Attributes
    public string Name {get; set;} = string.Empty;
    public int Capacity { get; set;}
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Description { get; set;}

#endregion
#region Calculated

    public ICollection<Reservation> Reservations { get; set; } 
        = new List<Reservation>();

#endregion
}