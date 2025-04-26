using System.Security.Claims;
using kvFileY.Api.Interfaces.Repository;
using kvFileY.Api.Interfaces.Service;
using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace kvFileY.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResult<ClaimsPrincipal>> RegisterUserAsync(
        UserCreateDto userCreate, 
        CancellationToken ct = default)
    {
        // Виправлено умову перевірки унікальності email
        if (!await _userRepository.IsEmailUniqueAsync(userCreate.Email, ct))
        {
            return new ServiceResult<ClaimsPrincipal>(
                false, 
                "User with this email already exists!");
        }

        var newUser = new User
        {
            Email = userCreate.Email,
            PasswordHash = CryptService.HashPassword(userCreate.Password),
            UserName = userCreate.UserName
        };

        await _userRepository.CreateAsync(newUser, ct);
        
        var claimsPrincipal = CreateClaimsPrincipal(newUser);
        await SignInAsync(claimsPrincipal);
        
        return new ServiceResult<ClaimsPrincipal>(
            true, 
            "User created successfully!", 
            claimsPrincipal);
    }

    public async Task<ServiceResult<ClaimsPrincipal>> LoginUserAsync(
        UserLoginDto userLogin, 
        CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(userLogin.Email, ct);
        
        if (user == null || 
            !CryptService.PasswordVerify(userLogin.Password, user.PasswordHash))
        {
            return new ServiceResult<ClaimsPrincipal>(
                false, 
                "Email or password is incorrect");
        }

        var claimsPrincipal = CreateClaimsPrincipal(user);
        await SignInAsync(claimsPrincipal);
        
        return new ServiceResult<ClaimsPrincipal>(
            true, 
            "Login successful!", 
            claimsPrincipal);
    }

    public async Task LogoutUserAsync(CancellationToken ct = default)
    {
        await _httpContextAccessor.HttpContext?.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme)!;
    }

    public bool CheckUserAuth() 
        => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
    
    private ClaimsPrincipal CreateClaimsPrincipal(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName)
            // Додайте додаткові claims за потреби
        };
        
        var identity = new ClaimsIdentity(
            claims, 
            CookieAuthenticationDefaults.AuthenticationScheme);
            
        return new ClaimsPrincipal(identity);
    }
    
    private async Task SignInAsync(ClaimsPrincipal principal)
    {
        await _httpContextAccessor.HttpContext?.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Тривалість сесії
            })!;
    }
}