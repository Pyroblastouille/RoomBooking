namespace RoomBooking.Api.Models;
public class User {

    #region Identifier
    public int Id { get; set; }

    #endregion


    #region Attributes
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    #endregion
    #region Calculated

    public ICollection<Reservation> Reservations { get; set; }
        = new List<Reservation>();

    #endregion

}

