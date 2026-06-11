using RoomBooking.Api.DTOs.Users;
namespace RoomBooking.Api.Interfaces;
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<bool> DeleteAsync(int id);
}