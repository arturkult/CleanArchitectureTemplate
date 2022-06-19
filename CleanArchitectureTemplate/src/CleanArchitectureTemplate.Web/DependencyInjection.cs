using CleanAchitectureTemplate.Web.Filters;
using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Web.Services;
using FluentValidation.AspNetCore;

namespace CleanArchitectureTemplate.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
                
        services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
        
        services.AddScoped<ICurrentUserService<Guid>, CurrentUserService>();

        return services;
    }
}