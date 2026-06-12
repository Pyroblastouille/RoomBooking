using Microsoft.EntityFrameworkCore;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomBooking.Infrastructure.Persistence.Repositories {
    public class BookingRepository : Repository<Booking>, IBookingRepository {
        public BookingRepository(AppDbContext context) : base(context) {
        }

        public async Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId) {
            var bookings = await _dbSet.Where(b => b.RoomId == roomId).ToListAsync();
            return bookings;
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId) {
            var bookings = await _dbSet
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.UserId == userId).ToListAsync();
            return bookings;
        }

        public new async Task<IEnumerable<Booking>> GetAllAsync() {
            var result = await _dbSet
                .Include(b => b.Room)
                .Include(b => b.User)
                .ToListAsync();
            return result;
        }

        public new async Task<Booking?> GetByIdAsync(int id) {
            var result = await _dbSet
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b=> b.Id == id);
            return result;
        }

        public async Task<bool> HasConflictAsync(int roomId, DateTime start, DateTime end, int? excludeId = null) {
            var result = await _dbSet.AnyAsync<Booking>(b =>
            b.RoomId == roomId &&
            b.Id != excludeId &&
            b.StartTime < end &&
            b.EndTime > start);
            return result;
        }

    }
}
