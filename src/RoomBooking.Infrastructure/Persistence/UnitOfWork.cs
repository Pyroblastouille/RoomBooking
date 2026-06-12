using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomBooking.Infrastructure.Persistence {
    public class UnitOfWork : IUnitOfWork {

        private readonly AppDbContext _appDbContext;

        public IRepository<Room> Rooms { get; }
        public IRepository<User> Users { get; }
        public IBookingRepository Bookings { get; }

        public UnitOfWork(AppDbContext context, IRepository<Room> roomRepo, IRepository<User> userRepo, IBookingRepository bookingRepo) {
            _appDbContext = context;
            Rooms = roomRepo;
            Users = userRepo;
            Bookings = bookingRepo;
        }
        public async Task<int> SaveChangesAsync() {
            return await _appDbContext.SaveChangesAsync();
        }

    }
}
