using SportField.BookingService;
using SportField.FieldService;
using SportField.FileService;
using SportField.IdentityService;
using SportField.NotificationService;

namespace SportField.WebAPI;

public static class ServicesRegistration
{
    public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFieldService(configuration);
        services.AddFileService(configuration);
        services.AddBookingService(configuration);
        services.AddIdentityService(configuration);
        services.AddNotificationService(configuration);
        return services;
    }
}
