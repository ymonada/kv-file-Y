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
        return Ok(res.Message);
    }
    [HttpPost("/auth/login")]
    public async Task<IActionResult> LoginUserAsync(UserLoginDto user, CancellationToken ct = default)
    {
        var res = await _authService.LoginUserAsync(user, ct);
        return Ok(res.Message);
    }
    [HttpGet("/auth/cool")]
    public Task<IActionResult> Private()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        if (userId == 0)
            return Task.FromResult<IActionResult>(NotFound("No authenticated user."));
        return Task.FromResult<IActionResult>(Ok(userId));
    }
}