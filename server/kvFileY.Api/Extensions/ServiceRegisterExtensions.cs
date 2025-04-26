using kvFileY.Api.Interfaces.Repository;
using kvFileY.Api.Interfaces.Service;
using kvFileY.Api.Repositories;
using kvFileY.Api.Services;
using kvFileY.Application.Services;

namespace kvFileY.Api.Extensions;

public static class ServiceRegisterExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileYRepository, FileYRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileYService, FileYService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddHttpContextAccessor();
        
        return services;
    }
}