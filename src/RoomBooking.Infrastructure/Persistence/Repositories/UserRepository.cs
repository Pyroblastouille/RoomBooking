using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Persistence.Repositories {
    public class UserRepository : Repository<User>, IUserRepository {
        public UserRepository(AppDbContext context) : base(context) {
        }

        public async Task<User?> GetByEmailAsync(string email) {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
