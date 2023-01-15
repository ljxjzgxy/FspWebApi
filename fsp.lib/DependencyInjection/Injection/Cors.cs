using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace fsp.lib.DependencyInjection.Individual;
public static class Cors
{
    public static IServiceCollection AddCustomCorsOrigins(
       this IServiceCollection services, string[]? origins = null)
    {
        if (origins == null || origins.Length == 0)
        {
            origins = new string[] { "http://192.168.1.171", "http://127.0.0.1:5173", "http://localhost:5173","http://127.0.0.1:3000", "http://localhost:3000" };
        }

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(p =>
            {
                p.WithOrigins(origins)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });
        });

        return services;
    }
}
