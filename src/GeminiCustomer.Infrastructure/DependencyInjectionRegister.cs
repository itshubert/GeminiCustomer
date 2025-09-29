using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Infrastructure.Interceptors;
using GeminiCustomer.Infrastructure.Persistence;
using GeminiCustomer.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiCustomer.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GeminiCustomerDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("GeminiCustomerDbContext");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}