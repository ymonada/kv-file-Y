using kvFileY.Api.Services;
using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Interfaces.Service;

public interface IUserService
{
    Task<ServiceResult<User>> GetUserAsync(int userId, CancellationToken ct = default);

    Task<ServiceResult<List<User>>> GetUserPageAsync(int page, int pageSize, CancellationToken ct = default);

    Task<ServiceResult<User>> CreateUserAsync(User userCreate);

    Task<ServiceResult<User>> UpdateUserAsync(UserUpdateDto userCreate, CancellationToken ct = default);

    Task<ServiceResult<bool>> DeleteUserAsync(int userId, CancellationToken ct = default);

}