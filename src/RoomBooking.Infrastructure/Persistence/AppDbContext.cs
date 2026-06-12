using Microsoft.EntityFrameworkCore;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Infrastructure.Persistence;

/// <summary>
/// Fais le lien entre les modèles C# et la bdd
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    #region Tables
    public DbSet<Room> Rooms {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Booking> Bookings {get; set;}

    #endregion

    #region Entity Config

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Config Room
        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(room => room.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(room => room.Description)
                .HasMaxLength(500);
            entity.Property(room => room.Capacity)
                .IsRequired();
        });

        //Config User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Email)
                .IsUnique();
            entity.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(user => user.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(user => user.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(user => user.PasswordHash)
                .IsRequired();
        });

        //Config Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(booking => booking.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(booking => booking.StartTime)
                .IsRequired();
            entity.Property(booking => booking.EndTime)
                .IsRequired();

            //Relation User
            entity.HasOne(booking => booking.User)
                .WithMany(user => user.Bookings)
                .HasForeignKey(booking => booking.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //Relation Room
            entity.HasOne(booking => booking.Room)
                .WithMany(room => room.Bookings)
                .HasForeignKey(booking => booking.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

        });
    }

    #endregion
}