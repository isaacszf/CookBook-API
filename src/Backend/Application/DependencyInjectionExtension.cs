using Application.Services.AutoMapper;
using Application.UseCases.Dashboard.Get;
using Application.UseCases.Recipe.Delete;
using Application.UseCases.Recipe.Filter;
using Application.UseCases.Recipe.Generate;
using Application.UseCases.Recipe.GetById;
using Application.UseCases.Recipe.Register;
using Application.UseCases.Recipe.Update;
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
        AddSqids(services, config);
        AddMapper(services);
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
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
        services.AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCaseUseCase>();
        services.AddScoped<IDeleteRecipeByIdUseCase, DeleteRecipeByIdUseCase>();
        services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
        services.AddScoped<IGenerateRecipeUseCase, GenerateRecipeUseCase>();

        services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
    }

    private static void AddSqids(IServiceCollection services, IConfiguration config)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = config.GetSection("Settings:IdCryptoAlphabet").Value!
        });

        services.AddSingleton(sqids);
    }
    
    private static void AddMapper(IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(opts =>
        {
            var sqids = option.GetService<SqidsEncoder<long>>()!;
            opts.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }
}