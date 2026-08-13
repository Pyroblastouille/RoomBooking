using RoomBooking.Application.DTOs.Rooms;
using RoomBooking.Application.DTOs.Users;
using RoomBooking.Application.Interfaces;
using RoomBooking.Domain.Entities;

namespace RoomBooking.Application.Services;

public class UserService : IUserService {
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IRepository<User> userRepository, IUnitOfWork uow, IPasswordHasher passwordHasher) {
        _userRepository = userRepository;
        _uow = uow;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync() {
        var users = await _userRepository.GetAllAsync();

        var result = new List<UserDto>();
        foreach (var user in users) {
            if (user is null) continue;
            result.Add(MapToDto(user));
        }
        return result;
    }


    public async Task<UserDto?> GetByIdAsync(int id) {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null) return null;

        return MapToDto(user);
    }

    public async Task<bool> CreateAsync(CreateUserDto dto) {
        var user = new User {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            CreatedAt = DateTime.UtcNow       // Géré par le serveur
        };

        var created = await _userRepository.AddAsync(user);
        await _uow.SaveChangesAsync();

        return created;
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto dto) {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null) return false;

        user.FirstName = dto.FirstName; 
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.PasswordHash = _passwordHasher.Hash(dto.Password);

        var updated = await _userRepository.UpdateAsync(id, user);
        await _uow.SaveChangesAsync();
        return updated;
    }

    public async Task<bool> DeleteAsync(int id) {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null) return false;

        var deleted = await _userRepository.DeleteAsync(user);
        await _uow.SaveChangesAsync();
        return deleted;
    }


    private UserDto MapToDto(User user) => new() {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        CreatedAt = user.CreatedAt,
        Email = user.Email,
        Role = user.Role,
    };
}
