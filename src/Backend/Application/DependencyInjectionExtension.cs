using Application.Services.AutoMapper;
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
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateProfileUseCase, UpdateProfileUseCase>();
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