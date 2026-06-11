using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Data;
using RoomBooking.Api.DTOs.Reservations;
using RoomBooking.Api.Interfaces;
using RoomBooking.Api.Models;
using RoomBooking.Api.Common;

namespace RoomBooking.Api.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;
    public ReservationService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(reservation => reservation.Room)
            .Include(reservation => reservation.User)
            .Select(reservation => MapToDto(reservation))
            .ToListAsync();
    }
    public async Task<ReservationDto?> GetByIdAsync(int id)
    {   
        var reservation = await _context.Reservations
            .Include(reservation => reservation.Room)
            .Include(reservation => reservation.User)
            .FirstOrDefaultAsync(reservation => reservation.Id == id);

        return reservation is null ? null : MapToDto(reservation);
    }
    public async Task<ServiceResult<ReservationDto>> CreateAsync(CreateReservationDto dto)
    {
        //Case Start after End
        if(dto.StartTime >= dto.EndTime)
            return ServiceResult<ReservationDto>.Fail("conflict between start date and end date.");
        //Case Reservation before now
        if(dto.StartTime < DateTime.UtcNow)
            return ServiceResult<ReservationDto>.Fail("conflict between start date and current date.");
        //Case Room exists and active
        var room = await _context.Rooms.FindAsync(dto.RoomId);
        if(room is null)
            return ServiceResult<ReservationDto>.Fail("Room does not exist.");
        if(!room.IsActive)
            return ServiceResult<ReservationDto>.Fail("Room is not active.");
        //User exists
        var user = await _context.Users.FindAsync(dto.UserId);
        if(user is null)
            return ServiceResult<ReservationDto>.Fail("User does not exist.");
        
        //Time Conflicts
        var hasConflict = await _context.Reservations.AnyAsync(reservation => 
            reservation.RoomId == dto.RoomId &&
            dto.StartTime < reservation.EndTime &&
            dto.EndTime > reservation.StartTime);
        if (hasConflict)
            return ServiceResult<ReservationDto>.Fail("Room already reserved.");
        
        //All good
        var reservation = new Reservation
        {
            Title = dto.Title,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            RoomId = dto.RoomId,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            User = user,
            Room = room
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return ServiceResult<ReservationDto>.Ok(MapToDto(reservation));
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation is null) return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    
    private static ReservationDto MapToDto(Reservation reservation) => new()
    {
        Id = reservation.Id,
        Title = reservation.Title,
        StartTime = reservation.StartTime,
        EndTime = reservation.EndTime,
        RoomId = reservation.RoomId,
        RoomName = (reservation.Room is null ? string.Empty : reservation.Room.Name),
        UserId = reservation.UserId,
        UserFullName = (reservation.User is null ? string.Empty : $"{reservation.User.FirstName} {reservation.User.LastName}"),
        CreatedAt = reservation.CreatedAt
    };
}