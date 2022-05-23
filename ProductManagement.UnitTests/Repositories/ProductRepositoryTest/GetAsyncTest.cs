using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Repositories.ProductRepositoryTest
{
    [ExcludeFromCodeCoverage]
    public sealed class GetAsyncTest : ProductRepositoryTestBase
    {
        [Fact]
        internal async Task EntityTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            Product product = await localDb.Products.FirstOrDefaultAsync();

            Product productDb = await repository.GetAsync(product.Id);

            Assert.Equal(product.ManufacturingDate, productDb.ManufacturingDate);
            Assert.Equal(product.ExpirationDate, productDb.ExpirationDate);
            Assert.Equal(product.Description, productDb.Description);
            Assert.Equal(product.Active, productDb.Active);
            Assert.Equal(product.ProviderId, productDb.ProviderId);
        }

        [Fact]
        internal async Task GenericTest()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);
            Product product = await localDb.Context.Products
                .Include(p => p.Provider)
                .AsNoTracking().OrderBy(p => Guid.NewGuid()).FirstOrDefaultAsync();

            ProductRead productDb = await repository.GetAsync<ProductRead>(product.Id);

            Assert.Equal(product.ManufacturingDate, productDb.ManufacturingDate);
            Assert.Equal(product.ExpirationDate, productDb.ExpirationDate);
            Assert.Equal(product.Description, productDb.Description);
            Assert.Equal(product.Id, productDb.Code);
            Assert.Equal(product.Active, productDb.Status.Equals(ProductStatus.Active));
            Assert.Equal(product.ProviderId, productDb.ProviderCode);
            Assert.Equal(product.Provider.Description, productDb.ProviderDescription);
            Assert.Equal(product.Provider.Phone, productDb.ProviderPhone);
        }
    }
}
