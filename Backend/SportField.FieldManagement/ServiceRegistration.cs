using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportField.FieldManagement.Infrastructure.Persistence;

namespace SportField.FieldManagement;

public static class ServiceRegistration
{
    public static void AddFieldServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FieldDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("SportField_FieldDB"));
        });
        //services.AddScoped<IFacilityRepository,>();
        //services.AddScoped<IFieldRepository,>();
        //services.AddScoped<IFieldUnitOfWork,>();
    }
}
