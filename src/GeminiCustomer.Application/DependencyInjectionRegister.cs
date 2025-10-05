using FluentValidation;
using GeminiCustomer.Application.Common.Behaviors;
using GeminiCustomer.Application.Common.Interfaces.Services;
using GeminiCustomer.Application.Users.Services;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiCustomer.Application;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddMappings();

        services.AddValidatorsFromAssembly(typeof(DependencyInjectionRegister).Assembly);

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddScoped<IUserService, UserService>();

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton(config);
        services.AddMapster();

        return services;
    }
}
