using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using ProductManagement.Interfaces;
using ProductManagement.Services;

namespace ProductManagement
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            MapperConfiguration mapperConfiguration = new(c => c.AddProfile<ProductManagementProfile>());
            mapperConfiguration.CompileMappings();
            return services
                .AddSingleton(mapperConfiguration.CreateMapper())
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductProviderService, ProductProviderService>();
        }
    }
}
