using System.Security.Claims;
using kvFileY.Api.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kvFileY.Api.Controllers;

[ApiController]
public class FileController : Controller
{
    private readonly IFileYService _fileYService;

    public FileController(IFileYService fileYService)
    {
        _fileYService = fileYService;
    }
    [Authorize]
    [HttpPost("/upload")]
    public async Task<IActionResult> UploadFilesAsync([FromForm] List<IFormFile> files,
        CancellationToken ct = default)
    {
        var res = await _fileYService.LoadFileArrayAsync(GetUserId(), files.ToList(), ct);
        return res.IsSuccess ? Ok() : BadRequest(res.Message);
    }
    [Authorize]
    [HttpGet("/filebyurl/{url}")]
    public async Task<IActionResult> GetFileUrlAsync([FromRoute] string url, CancellationToken ct = default)
    {
        var res = await _fileYService.GetFileByUrl(url, ct);
        return res.IsSuccess ? File(res.Data.FileBytes, res.Data.MineType, fileDownloadName: "aklsmc")
            : NotFound(res.Message);
    }
    [Authorize]
    [HttpGet("/filebyid/{id}")]
    public async Task<IActionResult> GetFileAsync(int id, CancellationToken ct = default)
    {
        var res = await _fileYService.GetFileAsync(id, ct);
        return res.IsSuccess ? Ok(res.Data) : BadRequest(res.Message);
    }
    [Authorize]
    [HttpGet("/file/page/")]
    public async Task<IActionResult> GetPageFilesAsync([FromQuery] int page,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortOrder = "asc", CancellationToken ct = default)
    {
        var res = await _fileYService.GetFilePageAsync(GetUserId(), page, pageSize, searchTerm, sortBy, sortOrder, ct);
        return res.IsSuccess ? Ok(res.Data) : BadRequest(res.Message);
    }
    [Authorize]
    [HttpDelete("file/delete")]
    public async Task<IActionResult> DeleteAll()
    {
        var res = await _fileYService.DeleteFileAsync(GetUserId());
        return Ok();
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
}
