using Microsoft.Extensions.DependencyInjection;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Application.Services;

namespace ComputerComponents.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IComponentService, ComponentService>();
            services.AddScoped<ISpecificationService, SpecificationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
