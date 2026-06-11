using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Data;
using RoomBooking.Api.DTOs.Rooms;
using RoomBooking.Api.Interfaces;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
    {
        return await _context.Rooms
            .Select(room => new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Description = room.Description,
                IsActive = room.IsActive,
                CreatedAt = room.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room is null) return null;

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Description = room.Description,
            IsActive = room.IsActive,
            CreatedAt = room.CreatedAt
        };
    }

    public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            Description = dto.Description,
            IsActive = true,                  // Activée par défaut
            CreatedAt = DateTime.UtcNow       // Géré par le serveur
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Description = room.Description,
            IsActive = room.IsActive,
            CreatedAt = room.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateRoomDto dto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room is null) return false;

        room.Name = dto.Name;
        room.Capacity = dto.Capacity;
        room.Description = dto.Description;
        room.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room is null) return false;

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }
}
