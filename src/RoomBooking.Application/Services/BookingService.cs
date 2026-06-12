// Services/BookingService.cs  ← règles métier ICI, pas dans Domain
using RoomBooking.Application.Common;
using RoomBooking.Application.DTOs.Bookings;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Services;

public class BookingService : IBookingService {
    private readonly IBookingRepository _bookingRepository;
    private readonly IRepository<Room> _roomRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _uow;

    public BookingService(IBookingRepository bookingRepo, IRepository<Room> roomRepo, IRepository<User> userRepo, IUnitOfWork uow) {
        _bookingRepository = bookingRepo;
        _roomRepository = roomRepo;
        _userRepository = userRepo;
        _uow = uow;
    }

    public async Task<IEnumerable<BookingDto>> GetAllAsync() {
        var bookings = await _bookingRepository.GetAllAsync();

        var result = new List<BookingDto>();
        foreach (var booking in bookings) {
            result.Add(MapToDto(booking));
        }
        return result;
    }
    public async Task<BookingDto?> GetByIdAsync(int id) {
        var booking = await _bookingRepository.GetByIdAsync(id);

        return booking is null ? null : MapToDto(booking);
    }
    public async Task<ServiceResult<BookingDto>> CreateAsync(CreateBookingDto dto) {
        //Case Start after End
        if (dto.StartTime >= dto.EndTime)
            return ServiceResult<BookingDto>.Fail("conflict between start date and end date.",409);
        //Case Booking before now
        if (dto.StartTime < DateTime.UtcNow)
            return ServiceResult<BookingDto>.Fail("conflict between start date and current date.", 409);
        //Case Room exists and active
        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room is null)
            return ServiceResult<BookingDto>.Fail("Room does not exist.",404);
        if (!room.IsActive)
            return ServiceResult<BookingDto>.Fail("Room is not active.",404);
        //User exists
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            return ServiceResult<BookingDto>.Fail("User does not exist.", 404);

        //Time Conflicts
        var hasConflict = await _bookingRepository.HasConflictAsync(dto.RoomId, dto.StartTime, dto.EndTime);
        if (hasConflict)
            return ServiceResult<BookingDto>.Fail("Room already reserved.", 409);

        //All good
        var booking = new Booking {
            Title = dto.Title,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            RoomId = dto.RoomId,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            User = user,
            Room = room
        };

        var success = await _bookingRepository.AddAsync(booking);
        await _uow.SaveChangesAsync();
        if (success) {
            return ServiceResult<BookingDto>.Ok(MapToDto(booking));
        } else {
            return ServiceResult<BookingDto>.Fail("Unexpected Error", 400);
        }
    }
    public async Task<ServiceResult<BookingDto>> UpdateAsync(int id, UpdateBookingDto dto) {
        //Case Start after End
        if (dto.StartTime >= dto.EndTime)
            return ServiceResult<BookingDto>.Fail("conflict between start date and end date.", 409);
        //Case Booking before now
        if (dto.StartTime < DateTime.UtcNow)
            return ServiceResult<BookingDto>.Fail("conflict between start date and current date.", 409);
        //Case Room exists and active
        var room = await _roomRepository.GetByIdAsync(dto.RoomId);
        if (room is null)
            return ServiceResult<BookingDto>.Fail("Room does not exist.", 404);
        if (!room.IsActive)
            return ServiceResult<BookingDto>.Fail("Room is not active.", 404);
        //User exists
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            return ServiceResult<BookingDto>.Fail("User does not exist.", 404);

        //Time Conflicts
        var hasConflict = await _bookingRepository.HasConflictAsync(dto.RoomId, dto.StartTime, dto.EndTime,id);
        if (hasConflict)
            return ServiceResult<BookingDto>.Fail("Room already reserved.", 409);

        //All good
        var booking = new Booking {
            Title = dto.Title,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            RoomId = dto.RoomId,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            User = user,
            Room = room
        };

        var success = await _bookingRepository.AddAsync(booking);
        await _uow.SaveChangesAsync();
        if (success) {
            return ServiceResult<BookingDto>.Ok(MapToDto(booking));
        } else {
            return ServiceResult<BookingDto>.Fail("Unexpected Error", 400);
        }
    }
    public async Task<bool> DeleteAsync(int id) {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if(booking == null) return false;
    
        var deleted = await _bookingRepository.DeleteAsync(booking);

        await _uow.SaveChangesAsync();
        return deleted;
    }



    private static BookingDto MapToDto(Booking booking) => new() {
        Id = booking.Id,
        Title = booking.Title,
        StartTime = booking.StartTime,
        EndTime = booking.EndTime,
        RoomId = booking.RoomId,
        RoomName = (booking.Room is null ? string.Empty : booking.Room.Name),
        UserId = booking.UserId,
        UserFullName = (booking.User is null ? string.Empty : $"{booking.User.FirstName} {booking.User.LastName}"),
        CreatedAt = booking.CreatedAt
    };
}
