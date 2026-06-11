namespace RoomBooking.Api.Models;
public class Reservation {
    #region Identifier
    public int Id { get; set; }
    #endregion
    #region ForeignKeys
    public int UserId { get; set; }
    public int RoomId {  get; set; }

    #endregion


    #region Attributes
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    #endregion
    #region Calculated

    public User User { get; set; } = null!;
    public Room Room { get; set; } = null!;


    #endregion
}

