using Application.Services.AutoMapper;
using Application.Services.Cryptography;
using Application.UseCases.User.Login.DoLogin;
using Application.UseCases.User.Profile;
using Application.UseCases.User.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration config)
    {
        AddMapper(services);
        AddPasswordEncrypt(services, config);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateProfileUseCase, UpdateProfileUseCase>();
    }

    private static void AddPasswordEncrypt(IServiceCollection services, IConfiguration config)
    {
        var encryptApiKey = config.GetSection("Settings:Password:EncryptAPIKey").Value;
        services.AddScoped(_ => new PasswordEncrypt(encryptApiKey!));
    }
    
    private static void AddMapper(IServiceCollection services)
    {
        var autoMapper = new AutoMapper.MapperConfiguration(opts =>
        {
            opts.AddProfile<AutoMapping>();
        }).CreateMapper();

        services.AddScoped(_ => autoMapper);
    }
}