using kvFileY.Domain.Entities;

namespace kvFileY.Application.DTOs;

public record UserDto(int Id, string UserName, string Email, string PasswordHash, ICollection<FileY> Files);
public record UserCreateDto(string UserName, string Email, string Password);
public record UserResponseDto(int Id, string Username, string Email);
public record UserLoginDto(string Email, string Password);
public record UserUpdateDto(string UserName, string Email);
