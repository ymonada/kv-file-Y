using System.Security.Claims;
using kvFileY.Api.Interfaces.Service;
using kvFileY.Api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kvFileY.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [Authorize]
    [HttpGet("/user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetUserId();
        if (userId == 0)
            return NotFound();
        var res = await _userService.GetUserAsync(userId);
        if (!res.IsSuccess)
            return NotFound(res.Message);
        return Ok(UserMapper.ToDto(res.Data));
    }
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
}
