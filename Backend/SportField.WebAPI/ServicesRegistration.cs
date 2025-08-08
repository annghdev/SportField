using SportField.FieldManagement;

namespace SportField.WebAPI;

public static class ServicesRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFieldServices(configuration);
        return services;
    }
}
