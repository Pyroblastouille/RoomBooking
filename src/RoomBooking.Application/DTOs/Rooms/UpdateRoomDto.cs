using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RoomBooking.Application.DTOs.Rooms;

public class UpdateRoomDto
{
    [Required(ErrorMessage ="Name required.")]
    [MaxLength(100)]
    public string Name{get; set;} = string.Empty;

    [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000.")]
    public int Capacity { get; set; }

    public bool IsActive {get; set;}

    [MaxLength(300)]
    public string? Description { get; set; }
}