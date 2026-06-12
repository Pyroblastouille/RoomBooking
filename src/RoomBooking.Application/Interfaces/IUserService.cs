using RoomBooking.Application.DTOs.Rooms;
using RoomBooking.Application.DTOs.Users;
namespace RoomBooking.Application.Interfaces;
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateUserDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateUserDto dto);
}