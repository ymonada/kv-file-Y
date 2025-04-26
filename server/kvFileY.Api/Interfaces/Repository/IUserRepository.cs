using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Interfaces.Repository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetUserPageAsync(int page, int pageSize, CancellationToken ct = default);
    
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default);
    
    Task<User> CreateAsync(User user, CancellationToken ct = default);
    Task<bool> UpdateAsync(UserUpdateDto user, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}