using kvFileY.Api.Interfaces.Repository;
using kvFileY.Api.Interfaces.Service;
using kvFileY.Api.Repositories;
using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ServiceResult<User>> GetUserAsync(int  userId,  CancellationToken ct = default)
    {
       var res = await _userRepository.GetByIdAsync(userId, ct);
       return res is null
           ? new ServiceResult<User>(false, "Not found User")
           : new ServiceResult<User>(true, "User", res);
    }
    public async Task<ServiceResult<List<User>>> GetUserPageAsync(int page, int pageSize,  CancellationToken ct = default)
    {
        var res = await _userRepository.GetUserPageAsync(page, pageSize, ct);
        return new ServiceResult<List<User>>(true, "Users list", res.ToList());
    }
    public async Task<ServiceResult<User>> CreateUserAsync(User userCreate)
    {
        var res = await _userRepository.CreateAsync(userCreate);
        return new ServiceResult<User>(true, "User Create",res);
    }
    public async Task<ServiceResult<User>> UpdateUserAsync(UserUpdateDto userCreate, CancellationToken ct = default)
    {
        var newUser = new User
        {
            Email = userCreate.Email,
            UserName = userCreate.UserName
        };
        var res = await _userRepository.CreateAsync(newUser, ct);
        return new ServiceResult<User>(true, "User", res);
    }

    public async Task<ServiceResult<bool>> DeleteUserAsync(int userId, CancellationToken ct = default)
    {
        var res = await _userRepository.DeleteAsync(userId, ct);
        return new ServiceResult<bool>(true, "User", res);
    }
    
}