using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using ProductManagement.Interfaces;
using ProductManagement.Services;

namespace ProductManagement
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceProvider)
        {
            MapperConfiguration mapperConfiguration = new(c => c.AddProfile<ProductManagementProfile>());
            mapperConfiguration.CompileMappings();
            return serviceProvider
                .AddScoped(p => mapperConfiguration.CreateMapper())
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProductProviderService, ProductProviderService>();
        }
    }
}
