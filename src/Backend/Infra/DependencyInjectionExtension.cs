using Domain.Repositories;
using Domain.Repositories.User;
using Domain.Security.Tokens;
using Domain.Services.LoggedUser;
using Infra.DataAccess;
using Infra.DataAccess.Repositories;
using Infra.Extensions;
using Infra.Security.Tokens.Access.Generator;
using Infra.Security.Tokens.Access.Validator;
using Infra.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        AddDbContext(services, config);
        AddRepos(services);
        AddTokens(services, config);
        AddServices(services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration config)
    {
        var connection = config.GetDefaultConnection();
        var version = new MariaDbServerVersion(new Version(11, 4, 2));
        
        services.AddDbContext<CookBookDbContext>(dbOptions =>
        {
            dbOptions.UseMySql(connection, version);
        });
    }
    
    private static void AddTokens(IServiceCollection services, IConfiguration config)
    {
        var expirationTimeInMinutes = 
            uint.Parse(config.GetSection("Settings:Jwt:ExpirationTimeMinutes").Value!);
        var signInKey = config.GetSection("Settings:Jwt:SignInKey").Value!;

        services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(expirationTimeInMinutes, signInKey));
        services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signInKey));
    }
    
    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }
    
    private static void AddRepos(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
    }
}