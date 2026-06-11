using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.Data;

/// <summary>
/// Fais le lien entre les modèles C# et la bdd
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    #region Tables
    public DbSet<Room> Rooms {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Reservation> Reservations {get; set;}

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

        //Config Reservation
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(reservation => reservation.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(reservation => reservation.StartTime)
                .IsRequired();
            entity.Property(reservation => reservation.EndTime)
                .IsRequired();

            //Relation User
            entity.HasOne(reservation => reservation.User)
                .WithMany(user => user.Reservations)
                .HasForeignKey(reservation => reservation.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //Relation Room
            entity.HasOne(reservation => reservation.Room)
                .WithMany(room => room.Reservations)
                .HasForeignKey(reservation => reservation.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

        });
    }

    #endregion
}