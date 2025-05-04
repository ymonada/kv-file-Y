using kvFileY.Api.DTOs;
using kvFileY.Api.Interfaces.Repository;
using kvFileY.Api.Interfaces.Service;
using kvFileY.Api.Services;
using kvFileY.Domain.Entities;
using Microsoft.AspNetCore.StaticFiles;

namespace kvFileY.Application.Services;

public class FileYService : IFileYService
{
    private readonly IFileYRepository _fileRepository;

    public FileYService(IFileYRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    private string GetMimeType(string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        return contentType;
    }

    public async Task<ServiceResult<FileY>> GetFileAsync(int fileId, CancellationToken ct = default)
    {
        var res = await _fileRepository.GetByIdAsync(fileId, ct);
        return res != null
            ? new ServiceResult<FileY>(true, fileId.ToString(), res)
            : new ServiceResult<FileY>(false, "File not found");
    }

    public async Task<ServiceResult<List<FileY>>> GetFilePageAsync(int userId,
        int page,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortOrder = "asc",
        CancellationToken ct = default)
    {
        if (page < 1)
            page = 1;
        if (pageSize < 1)
            pageSize = 10;
        var res = await _fileRepository.GetPageAsync(userId, page, pageSize, searchTerm, sortBy, sortOrder, ct);
        return new ServiceResult<List<FileY>>(true, "files", res);
    }
    public async Task<ServiceResult<List<FileY>>> LoadFileArrayAsync(int userId, List<IFormFile> files, CancellationToken ct = default)
    {
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
        var userFiles = new List<FileY>();
        foreach (var file in files)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            using var stream1 = file.OpenReadStream();
            byte[] hash = await md5.ComputeHashAsync(stream1);
            var level0 = Guid.NewGuid();
            int level1 = hash[0] % 256 % 10;
            int level2 = hash[1] % 256 % 10;
            string nestedDirectory = Path.Combine(basePath
                , level1.ToString()
                , level2.ToString());
            if (!Directory.Exists(nestedDirectory))
            {
                Directory.CreateDirectory(nestedDirectory);
            }
            var newFileName = file.FileName.Replace(' ', '_');
            var filePath = Path.Combine(nestedDirectory, newFileName);
            var newFilePath = Path.Combine("files", level1.ToString(), level2.ToString(), file.FileName.Replace(' ', '_'));
            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            userFiles.Add(new FileY()
            {
                UserId = userId,
                FileName = newFileName,
                FilePath = newFilePath,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow,
                FileSize = file.Length,

            });
        }

        if (userFiles.Count < 1)
        {
            return new ServiceResult<List<FileY>>(false, "No files");
        }
        var res = await _fileRepository.CreateRangeAsync(userFiles, ct);
        return new ServiceResult<List<FileY>>(true, "Uploaded files", res);
    }
    public async Task<ServiceResult<UserFileDto>> GetFileByUrl(string path, CancellationToken ct = default)
    {
        string decodedPath = Uri.UnescapeDataString(path);
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", decodedPath.TrimStart('/'));
        if (!File.Exists(filePath))
        {
            return new ServiceResult<UserFileDto>(false, "File not found");
        }
        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        string mimeType = GetMimeType(filePath);
        return new ServiceResult<UserFileDto>(true, "File", new UserFileDto(fileBytes, mimeType));
    }
    public async Task<ServiceResult<FileY>> UpdateFileAsync(FileYUpdateDto fileUpdate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<bool>> DeleteFileAsync(int id, CancellationToken ct = default)
    {

        var res = await _fileRepository.DeleteAllAsync(ct);
        return new ServiceResult<bool>(true, "Uploaded files", res);
    }
}
