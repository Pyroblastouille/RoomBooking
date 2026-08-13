using RoomBooking.Application.Common;
using RoomBooking.Application.DTOs.Auth;

namespace RoomBooking.Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto);
}
