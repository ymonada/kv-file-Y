using System.Security.Claims;
using kvFileY.Api.Services;
using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Interfaces.Service;

public interface IAuthService
{
    Task<ServiceResult<ClaimsPrincipal>> RegisterUserAsync(UserCreateDto user, CancellationToken ct = default);
    Task<ServiceResult<ClaimsPrincipal>> LoginUserAsync(UserLoginDto userLogin, CancellationToken ct = default);
    Task LogoutUserAsync(CancellationToken ct = default);
    bool CheckUserAuth();
}

