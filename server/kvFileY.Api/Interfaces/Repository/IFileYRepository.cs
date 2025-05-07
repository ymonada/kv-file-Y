using System.Linq.Expressions;
using kvFileY.Api.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Interfaces.Repository;

public interface IFileYRepository
{
    Task<List<FileY>> GetPageAsync(int userId,
        int page = 1,
        int pageSize = 15,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortOrder = "asc",
        CancellationToken ct = default);
    Task<FileY?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<FileY?> GetByStoredNameAsync(string storedName, CancellationToken ct = default);
    Task<long> GetFilesCountAsync(int userId, CancellationToken ct = default);
    Task<List<FileY>> CreateRangeAsync(List<FileY> files, CancellationToken ct = default);
    // Перевірки
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> IsOwnerAsync(int fileId, int userId, CancellationToken ct = default);
    Task<FileY> CreateAsync(FileY user, CancellationToken ct = default);
    Task<bool> UpdateAsync(FileYUpdateDto user, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    // Додаткові операції
    Task<int> GetFileCountAsync(int userId, CancellationToken ct = default);

    Task<bool> DeleteAllAsync(CancellationToken ct = default);
}
