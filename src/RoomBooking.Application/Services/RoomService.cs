
using RoomBooking.Application.DTOs.Bookings;
using RoomBooking.Application.DTOs.Rooms;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Services;

public class RoomService : IRoomService {
    private readonly IRepository<Room> _roomRepository;
    private readonly IUnitOfWork _uow;

    public RoomService(IRepository<Room> roomRepository, IUnitOfWork uow)
    {
        _roomRepository = roomRepository;
        _uow = uow;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync() {
        var rooms = await _roomRepository.GetAllAsync();

        var result = new List<RoomDto>();
        foreach (var room in rooms) {
            if (room is null) continue;
            result.Add(MapToDto(room));
        }
        return result;
    }


    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room is null) return null;

        return MapToDto(room);
    }

    public async Task<bool> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            Description = dto.Description,
            IsActive = true,                  // Activée par défaut
            CreatedAt = DateTime.UtcNow       // Géré par le serveur
        };

        var created = await _roomRepository.AddAsync(room);
        await _uow.SaveChangesAsync();

        return created;
    }

    public async Task<bool> UpdateAsync(int id, UpdateRoomDto dto)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null) return false;

        room.Name = dto.Name;
        room.Capacity = dto.Capacity;
        room.Description = dto.Description;
        room.IsActive = dto.IsActive;

        var updated = await _roomRepository.UpdateAsync(id, room);
        await _uow.SaveChangesAsync();
        return updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null) return false;

        var deleted = await _roomRepository.DeleteAsync(room);
        await _uow.SaveChangesAsync();
        return deleted;
    }


    private RoomDto MapToDto(Room room) => new() {
        Id = room.Id,
        IsActive = room.IsActive,
        Capacity = room.Capacity,
        CreatedAt = room.CreatedAt,
        Description = room.Description,
        Name = room.Name
    };
}
