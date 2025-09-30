using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Application.Common.Interfaces.Authentication;
using GeminiCustomer.Application.Common.Interfaces.Services;
using GeminiCustomer.Infrastructure.Authentication;
using GeminiCustomer.Infrastructure.Interceptors;
using GeminiCustomer.Infrastructure.Persistence;
using GeminiCustomer.Infrastructure.Persistence.Repositories;
using GeminiCustomer.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = JwtHelper.GetTokenValidationParameters(
                    jwtSettings.Issuer,
                    jwtSettings.Audiences,
                    jwtSettings.Secret));

        return services;
    }
}