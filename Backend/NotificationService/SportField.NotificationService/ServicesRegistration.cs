using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SportField.FieldService;

public static class ServicesRegistration
{
    public static IServiceCollection AddFieldService(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
