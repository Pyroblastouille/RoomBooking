using RoomBooking.Infrastructure;
using RoomBooking.Application.Services;
using RoomBooking.Application.Interfaces;
using RoomBooking.Api.Interfaces;
using RoomBooking.Api.Common;

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
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IHttpResponseHandler, HttpResponseHandler>();


//Connexion PostgreSQL -> DbContext
//Take element "ConnectionStrings > DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' est introuvable dans la configuration.");
}

builder.Services.AddInfrastructure(connectionString: connectionString);

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
