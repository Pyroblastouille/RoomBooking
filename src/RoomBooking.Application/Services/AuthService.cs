using RoomBooking.Application.Common;
using RoomBooking.Application.DTOs.Auth;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Services;

public class AuthService : IAuthService {
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _uow;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IUnitOfWork uow) {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _uow = uow;
    }

    public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto dto) {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing is not null)
            return ServiceResult<AuthResponseDto>.Fail("Email already in use.", 409);

        var user = new User {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Role = Role.User,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.AddAsync(user);
        if (!created)
            return ServiceResult<AuthResponseDto>.Fail("Unable to register user.", 400);

        await _uow.SaveChangesAsync();

        return ServiceResult<AuthResponseDto>.Ok(BuildAuthResponse(user));
    }

    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto) {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            return ServiceResult<AuthResponseDto>.Fail("Invalid email or password.", 401);

        return ServiceResult<AuthResponseDto>.Ok(BuildAuthResponse(user));
    }

    private AuthResponseDto BuildAuthResponse(User user) {
        var (token, expiresAt) = _tokenService.GenerateToken(user);

        return new AuthResponseDto {
            Token = token,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role
        };
    }
}
