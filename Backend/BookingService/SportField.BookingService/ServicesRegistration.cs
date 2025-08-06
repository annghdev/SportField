using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportField.BookingService.Infrastructure.Persistence;

namespace SportField.BookingService;

public static class ServicesRegistration
{
    public static IServiceCollection AddBookingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingServiceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("BookingServiceDb"));
        });
        return services;
    }
}
