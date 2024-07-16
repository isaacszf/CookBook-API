using Microsoft.Extensions.Configuration;

namespace Infra.Extensions;

public static class ConfigurationExtension
{
    public static string GetDefaultConnection(this IConfiguration config) => 
        config.GetConnectionString("Connection")!;
}