using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Interfaces {
    public interface IBookingRepository : IRepository<Booking> {
        Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        Task<bool> HasConflictAsync(int roomId, DateTime start, DateTime end, int? excludeId = null);
    }
}
