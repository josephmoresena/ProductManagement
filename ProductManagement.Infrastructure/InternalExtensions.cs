using System;
using System.Linq;

using ProductManagement.Entities;
using ProductManagement.Objects;

namespace ProductManagement.Infrastructure
{
    internal static class InternalExtensions
    {
        public static IQueryable<Product> Filter(this IQueryable<Product> source, ProductFilter filter)
            => filter is null ? source : Filter(filter, source);
        public static IQueryable<TEntity> Limit<TEntity>(this IQueryable<TEntity> source, LimitSettings limit) where TEntity : class
            => source.Skip(GetSkip(limit)).Take(limit.Limit);

        private static Int32 GetSkip(LimitSettings limit) => limit.Page == default ? 0 : (limit.Page - 1) * limit.Limit;
        private static IQueryable<Product> Filter(ProductFilter filter, IQueryable<Product> source)
        {
            IQueryable<Product> result = source;
            if (filter.Products.Any())
                result = result.Where(p => filter.Products.Contains(p.Id));

            if (filter.Providers.Any())
                result = result.Where(p => filter.Providers.Contains(p.Provider.Id));

            if (!String.IsNullOrEmpty(filter.ProductDescription))
                if (filter.ExactMatchProductDescription)
                    result = result.Where(p => p.Description.Equals(filter.ProductDescription));
                else
                    result = result.Where(p => p.Description.Contains(filter.ProductDescription));

            if (!String.IsNullOrEmpty(filter.ProviderDescription))
                if (filter.ExactMatchProviderDescription)
                    result = result.Where(p => p.Provider.Description.Equals(filter.ProviderDescription));
                else
                    result = result.Where(p => p.Provider.Description.Contains(filter.ProviderDescription));

            if (filter.Status.HasValue)
                result = result.Where(p => p.Active.Equals(filter.Status.Equals(ProductStatus.Active)));

            return result;
        }
    }
}
