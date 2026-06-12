using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoomBooking.Application.Interfaces;
using RoomBooking.Infrastructure.Persistence;
using RoomBooking.Infrastructure.Persistence.Repositories;
using System.ComponentModel.Design;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString) {
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRepository<Room>, Repository<Room>>();
        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
