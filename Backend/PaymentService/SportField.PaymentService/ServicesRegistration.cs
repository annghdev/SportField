using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SportField.BookingService;

public static class ServicesRegistration
{
    public static IServiceCollection AddPaymentService(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
