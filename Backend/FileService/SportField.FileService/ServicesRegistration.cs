using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SportField.FileService;

public static class ServicesRegistration
{
    public static IServiceCollection AddFileService(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
