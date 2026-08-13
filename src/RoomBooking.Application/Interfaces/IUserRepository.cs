using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Interfaces {
    public interface IUserRepository : IRepository<User> {
        Task<User?> GetByEmailAsync(string email);
    }
}
