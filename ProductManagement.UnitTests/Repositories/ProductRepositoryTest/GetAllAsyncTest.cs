using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper.QueryableExtensions;

using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Repositories.ProductRepositoryTest
{
    [ExcludeFromCodeCoverage]
    public sealed class GetAllAsyncTest : ProductRepositoryTestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task SimpleTest(Boolean invalidPage)
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            ProductFilter filter = new()
            {
                ProductDescription = "6",
                ProviderDescription = "1",
                Status = ProductStatus.Active,
            };
            LimitSettings limit = new()
            {
                Page = invalidPage ? 0 : 2,
                Limit = 5,
            };

            ISet<ProductRead> products = Filter(filter, localDb.Products.OrderBy(p => p.Id))
                .Skip(limit.Page == default ? 0 : (limit.Page - 1) * limit.Limit)
                .Take(limit.Limit).ProjectTo<ProductRead>(mapper.ConfigurationProvider)
                .ToHashSet();

            await foreach (ProductRead read in repository.GetAllAsync<ProductRead>(limit, filter: filter))
                Assert.Contains(read, products);
        }
    }
}
