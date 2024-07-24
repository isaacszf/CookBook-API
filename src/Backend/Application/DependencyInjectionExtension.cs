using Application.Services.AutoMapper;
using Application.UseCases.Recipe.Register;
using Application.UseCases.User.ChangePassword;
using Application.UseCases.User.Login.DoLogin;
using Application.UseCases.User.Profile;
using Application.UseCases.User.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration config)
    {
        AddMapper(services, config);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateProfileUseCase, UpdateProfileUseCase>();
        
        services.AddScoped<IChangeUserPasswordUseCase, ChangeUserPasswordUseCase>();

        services.AddScoped<ICreateRecipeUseCase, CreateRecipeUseCase>();
    }
    
    private static void AddMapper(IServiceCollection services, IConfiguration config)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = config.GetSection("Settings:IdCryptoAlphabet").Value!
        });
        
        var autoMapper = new AutoMapper.MapperConfiguration(opts =>
        {
            opts.AddProfile(new AutoMapping(sqids));
        }).CreateMapper();

        services.AddScoped(_ => autoMapper);
    }
}