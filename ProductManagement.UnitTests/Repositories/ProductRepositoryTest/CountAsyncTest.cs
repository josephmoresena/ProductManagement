using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Repositories.ProductRepositoryTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CountAsyncTest : ProductRepositoryTestBase
    {
        [Fact]
        public async void SimpleTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            Int32 count = await localDb.Products.CountAsync();
            Assert.Equal(count, await repository.CountAsync());
        }

        [Fact]
        public async void SimpleFilterTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            ProductFilter filter = new()
            {
                ProductDescription = "6",
                ProviderDescription = "1",
                Status = ProductStatus.Active,
            };
            Int32 count = await Filter(filter, localDb.Products).CountAsync();
            Assert.Equal(count, await repository.CountAsync(filter: filter));
        }

        [Fact]
        public async void ExactProductFilterTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            ProductFilter filter = new()
            {
                ProductDescription = await localDb.Products.Select(p => p.Description).FirstOrDefaultAsync(),
                ExactMatchProductDescription = true,
            };
            Int32 count = await Filter(filter, localDb.Context.Products).CountAsync();
            Assert.Equal(count, await repository.CountAsync(filter: filter));
        }

        [Fact]
        public async void ExactProviderFilterTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            ProductFilter filter = new()
            {
                ProviderDescription = await localDb.Providers.Select(p => p.Description).FirstOrDefaultAsync(),
                ExactMatchProviderDescription = true,
            };
            Int32 count = await Filter(filter, localDb.Context.Products).CountAsync();
            Assert.Equal(count, await repository.CountAsync(filter: filter));
        }

        [Fact]
        public async void ExactCodeFilterTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            ProductFilter filter = new()
            {
                Products = await localDb.Products.Select(p => p.Id).Take(5).ToArrayAsync(),
            };
            Int32 count = await Filter(filter, localDb.Context.Products).CountAsync();
            Assert.Equal(count, await repository.CountAsync(filter: filter));
        }

        [Fact]
        public async void ExactProviderCodeFilterTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            ProductFilter filter = new()
            {
                Providers = await localDb.Providers.Select(p => p.Id).Take(2).ToArrayAsync(),
            };
            Int32 count = await Filter(filter, localDb.Context.Products).CountAsync();
            Assert.Equal(count, await repository.CountAsync(filter: filter));
        }
    }
}
