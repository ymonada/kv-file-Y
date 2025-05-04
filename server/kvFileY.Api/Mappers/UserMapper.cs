using kvFileY.Application.DTOs;
using kvFileY.Domain.Entities;

namespace kvFileY.Api.Mappers;

public static class UserMapper
{
    public static UserResponseDto ToDto(User user) =>
        new UserResponseDto(user.Id, user.UserName, user.Email);

}
