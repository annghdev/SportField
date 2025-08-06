using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportField.FieldService.Infrastructure.Persistence;

namespace SportField.FieldService;

public static class ServicesRegistration
{
    public static IServiceCollection AddFieldService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FieldServiceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("FieldServiceDb"));
        });
        return services;
    }
}
