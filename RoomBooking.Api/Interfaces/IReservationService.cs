using RoomBooking.Api.DTOs.Reservations;
using RoomBooking.Api.Common;

namespace RoomBooking.Api.Interfaces;
public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllAsync();
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<ServiceResult<ReservationDto>> CreateAsync(CreateReservationDto dto);
    Task<bool> DeleteAsync(int id);
}