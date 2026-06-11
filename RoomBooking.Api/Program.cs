using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Data;
using RoomBooking.Api.Interfaces;
using RoomBooking.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Connexion PostgreSQL -> DbContext
//Take element "ConnectionStrings > DefaultConnection"
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
