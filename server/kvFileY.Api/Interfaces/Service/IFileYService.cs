using kvFileY.Api.DTOs;
using kvFileY.Api.Services;
using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Interfaces.Service;

public interface IFileYService 
{
    Task<ServiceResult<FileY>> GetFileAsync(int fileId, CancellationToken ct = default);

    Task<ServiceResult<List<FileY>>> GetFilePageAsync(int userId,
        int page,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortOrder = "asc",
        CancellationToken ct = default);
    
    Task<ServiceResult<List<FileY>>> LoadFileArrayAsync(int userId, List<IFormFile> files, CancellationToken ct = default);
    Task<ServiceResult<UserFileDto>> GetFileByUrl(string path, CancellationToken ct = default);
    Task<ServiceResult<FileY>> UpdateFileAsync(FileYUpdateDto fileUpdate, CancellationToken ct = default);

    Task<ServiceResult<bool>> DeleteFileAsync(int fileId, CancellationToken ct = default);
}