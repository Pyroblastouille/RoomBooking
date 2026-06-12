using RoomBooking.Application.DTOs.Rooms;
namespace RoomBooking.Application.Interfaces;
public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync();
    Task<RoomDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateRoomDto dto);
    Task<bool> UpdateAsync(int id, UpdateRoomDto dto);
    Task<bool> DeleteAsync(int id);
}