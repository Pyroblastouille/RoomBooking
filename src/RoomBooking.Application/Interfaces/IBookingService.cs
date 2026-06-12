using RoomBooking.Application.DTOs.Bookings;
using RoomBooking.Application.Common;

namespace RoomBooking.Application.Interfaces;
public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllAsync();
    Task<BookingDto?> GetByIdAsync(int id);
    Task<ServiceResult<BookingDto>> CreateAsync(CreateBookingDto dto);
    Task<ServiceResult<BookingDto>> UpdateAsync(int id,UpdateBookingDto dto);
    Task<bool> DeleteAsync(int dto);
}