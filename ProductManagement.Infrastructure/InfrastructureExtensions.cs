using Microsoft.Extensions.DependencyInjection;

using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Interfaces;

namespace ProductManagement.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceProvider)
            => serviceProvider.AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IProductProviderRepostory, ProductProviderRepostory>();
    }
}
