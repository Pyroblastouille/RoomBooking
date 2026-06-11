using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Data;
using RoomBooking.Api.Interfaces;
using RoomBooking.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Room Booking API",
        Version = "v1",
        Description = "API pour la gestion des réservations de salles",
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "G. MRT",
            Email = "contact@pyroblastouille.com"
        }
    });
});

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", policy =>
   {
       policy.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader();
   });
});

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IHttpResponseHandler, HttpResponseHandler>();


//Connexion PostgreSQL -> DbContext
//Take element "ConnectionStrings > DefaultConnection"
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
