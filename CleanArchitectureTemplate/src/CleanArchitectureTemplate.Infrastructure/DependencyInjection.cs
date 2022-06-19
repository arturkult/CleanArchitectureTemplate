using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Interfaces;
using CleanArchitectureTemplate.Infrastructure.DateTime;
using CleanArchitectureTemplate.Infrastructure.EventDispatchers;
using CleanArchitectureTemplate.Infrastructure.Persistence.EntityFramework;
using CleanArchitectureTemplate.Infrastructure.Persistence.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        else
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


        return services;
    }
}