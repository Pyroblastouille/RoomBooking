using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Application.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "Email required.")]
    [EmailAddress(ErrorMessage = "Email not valid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password required.")]
    public string Password { get; set; } = string.Empty;
}
