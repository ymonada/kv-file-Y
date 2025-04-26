using System.Linq.Expressions;
using kvFileY.Api.DTOs;
using kvFileY.Api.Interfaces.Repository;
using kvFileY.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace kvFileY.Api.Repositories;

public class FileYRepository : IFileYRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<FileYRepository> _logger;

    public FileYRepository(AppDbContext context, ILogger<FileYRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<List<FileY>> GetPageAsync(int userId,
        int page = 1,
        int pageSize = 15,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortOrder = "asc", 
        CancellationToken ct = default)
    {
        var query = _context.Files
            .AsNoTracking()
            .Where(f => f.UserId == userId);

        // Додатковий пошук (якщо вказано)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            query = query.Where(f => 
                f.FileName.ToLower().Contains(search));
        }

        // Динамічне сортування (як у GetByFilter)
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var selector = GetSortSelector(sortBy);
            query = sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(selector) 
                : query.OrderBy(selector);
        }
        else
        {
            // Сортування за замовчуванням, якщо не вказано
            query = query.OrderBy(f => f.Id);
        }

        // Пагінація та виконання запиту
        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<FileY?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }

    public async Task<FileY?> GetByStoredNameAsync(string storedName, CancellationToken ct = default)
    {
        return await _context.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x
                .FileName.ToLower()
                .Contains(storedName.ToLower()), cancellationToken: ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _context.Files.AnyAsync(x => x.Id == id, ct);
    }

    public async  Task<bool> IsOwnerAsync(int fileId, int userId, CancellationToken ct = default)
    {
        return await _context.Files
            .AsNoTracking()
            .Where(f => f.Id == fileId)
            .Select(f => f.UserId)
            .FirstOrDefaultAsync(ct) == userId;
    }
    public async Task<bool> UpdateAsync(FileYUpdateDto file, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        var updatedRows = await _context.Files
            .Where(f => f.Id == file.Id)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(f => f.FileName, file.FileName)
                    .SetProperty(f => f.ContentType, file.ContentType)
                    .SetProperty(f => f.FilePath, file.FilePath)
                    .SetProperty(f => f.FileSize, file.FileSize),
                ct);

        return updatedRows > 0;
    }

    public async  Task<FileY> CreateAsync(FileY file, CancellationToken ct = default)
    {
       await _context.Files.AddAsync(file, ct);
       await _context.SaveChangesAsync(ct);
       return file;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var affectedRows = await _context.Files
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync(ct);

            return affectedRows > 0; // Повертає true, якщо щось було видалено
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"Помилка при видаленні файлу {id}");
            return false;
        } 
    } 

    public async  Task<int> GetFileCountAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Files.AsNoTracking().Where(f => f.UserId == userId).CountAsync(ct);
    }

    private static Expression<Func<FileY, object>> GetSortSelector(string sortBy)
    {
        return sortBy.ToLower() switch
        {
            "name" => f => f.FileName,
            "size" => f => f.FileSize,
            "date" => f => f.UploadedAt,
            _ => f => f.Id // Сортування за замовчуванням
        };
    }
   
}
