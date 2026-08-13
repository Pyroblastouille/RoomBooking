using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
