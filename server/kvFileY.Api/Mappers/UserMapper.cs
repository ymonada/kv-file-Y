using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)=> 
        new UserDto(user.Id, user.UserName, user.Email, user.PasswordHash, user.Files);
    
    
}