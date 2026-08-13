using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomBooking.Application.Interfaces;
using RoomBooking.Infrastructure.Persistence;
using RoomBooking.Infrastructure.Persistence.Repositories;
using RoomBooking.Infrastructure.Security;
using System.ComponentModel.Design;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, IConfiguration configuration) {
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRepository<Room>, Repository<Room>>();
        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
