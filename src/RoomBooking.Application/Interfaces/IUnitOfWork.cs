using RoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomBooking.Application.Interfaces {
    public interface IUnitOfWork {
        IRepository<Room> Rooms { get; }
        IRepository<User> Users { get; }
        IBookingRepository Bookings { get; }
        Task<int> SaveChangesAsync();
    }
}
