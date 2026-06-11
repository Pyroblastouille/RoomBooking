using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RoomBooking.Api.DTOs.Users;

public class CreateUserDto
{
    [Required(ErrorMessage ="FirstName required.")]
    [MaxLength(50)]
    public string FirstName{get; set;} = string.Empty;

    [Required(ErrorMessage ="LastName required.")]
    [MaxLength(50)]
    public string LastName{get; set;} = string.Empty;

    [Required(ErrorMessage ="Email required.")]
    [EmailAddress(ErrorMessage = "Email not valid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password required.")]
    [MinLength(8, ErrorMessage = "Password must contain at least 8 characters.")]
    public string Password { get; set; } = string.Empty;
}