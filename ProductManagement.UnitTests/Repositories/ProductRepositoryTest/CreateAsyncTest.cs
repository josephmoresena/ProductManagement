using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using AutoFixture;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Entities;
using ProductManagement.Interfaces;

using Xunit;

namespace ProductManagement.UnitTests.Repositories.ProductRepositoryTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CreateAsyncTest : ProductRepositoryTestBase
    {
        [Fact]
        internal async Task Test()
        {
            using ILocalDbContext localDb = CreateContext(false);
            IProductRepository repository = base.GetRepository(localDb.Context);

            ProductProvider provider = await localDb.Providers.FirstOrDefaultAsync();
            Product product = fixture.Build<Product>()
                .Without(p => p.Id).With(p => p.ProviderId, provider.Id).Create();

            Int32 result = await repository.CreateAsync(product);
            Product productDb = await localDb.Products.Include(p => p.Provider)
                .SingleOrDefaultAsync(p => p.Id.Equals(result));

            Assert.NotNull(productDb);
            Assert.Equal(product.ManufacturingDate, productDb.ManufacturingDate);
            Assert.Equal(product.ExpirationDate, productDb.ExpirationDate);
            Assert.Equal(product.Description, productDb.Description);
            Assert.Equal(product.Active, productDb.Active);
            Assert.Equal(product.ProviderId, productDb.ProviderId);
            Assert.Equal(provider.Id, productDb.Provider.Id);
            Assert.Equal(provider.Description, productDb.Provider.Description);
            Assert.Equal(provider.Phone, productDb.Provider.Phone);
        }
    }
}
