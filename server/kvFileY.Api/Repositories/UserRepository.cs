using kvFileY.Api.Interfaces.Repository;
using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace kvFileY.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<IReadOnlyList<User>> GetUserPageAsync(int page, int pageSize = 15, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((page-1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id, ct);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default)
    {
       return !await _context.Users
           .AsNoTracking()
           .AnyAsync(u => u.Email == email, ct);
    }

    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        await  _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
        return user;
    }

    public async Task<bool> UpdateAsync(UserUpdateDto user, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        await _context.Users.ExecuteUpdateAsync(x=>x
            .SetProperty(u=>u.UserName,user.UserName)
            .SetProperty(u=>u.Email,user.Email),ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            int affectedRows = await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync(ct);

            return affectedRows > 0; // Повертає true, якщо щось було видалено
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Помилка при видаленні користувача {id}");
            return false;
        }
    }
}