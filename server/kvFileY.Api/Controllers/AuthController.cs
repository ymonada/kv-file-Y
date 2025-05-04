using System.Security.Claims;
using kvFileY.Api.Interfaces.Service;
using kvFileY.Api.Services;
using kvFileY.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace kvFileY.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("/auth/register")]
    public async Task<IActionResult> RegisterUserAsync(UserCreateDto user, CancellationToken ct =  default)
    {
        var res = await _authService.RegisterUserAsync(user, ct);
        return res.IsSuccess ?  Ok(res.Message) : BadRequest(res.Message);
    }
    [HttpPost("/auth/login")]
    public async Task<IActionResult> LoginUserAsync(UserLoginDto user, CancellationToken ct = default)
    {
        var res = await _authService.LoginUserAsync(user, ct);
        return res.IsSuccess ?  Ok(res.Message) : BadRequest(res.Message);
    }
    [HttpGet("/auth/check")]
    public Task<IActionResult> CheckAuth()
    {
        if (GetUserId == 0)
             return Task.FromResult<IActionResult>(Ok(false));
        return Task.FromResult<IActionResult>(Ok(true));
    }
    [HttpPost("/auth/logout")]
    public async Task LogoutUserAsync(CancellationToken ct = default)
    {
        await _authService.LogoutUserAsync(ct);
    }
    private int GetUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
}