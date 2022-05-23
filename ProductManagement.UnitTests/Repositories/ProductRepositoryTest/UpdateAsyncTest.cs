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
    public sealed class UpdateAsyncTest : ProductRepositoryTestBase
    {
        [Fact]
        internal async Task Test()
        {
            using ILocalDbContext localDb = CreateContext(true);
            IProductRepository repository = base.GetRepository(localDb.Context);

            Product product = await localDb.Products.FirstOrDefaultAsync();

            product.Description = fixture.Create<String>();
            product.ManufacturingDate = fixture.Create<DateTime>();
            product.ExpirationDate = fixture.Create<DateTime>();

            await repository.UpdateAsync(product);

            Product productDb = await localDb.Products.Include(p => p.Provider)
                .SingleOrDefaultAsync(p => p.Id.Equals(product.Id));

            Assert.Equal(product.ManufacturingDate, productDb.ManufacturingDate);
            Assert.Equal(product.ExpirationDate, productDb.ExpirationDate);
            Assert.Equal(product.Description, productDb.Description);
            Assert.Equal(product.Active, productDb.Active);
            Assert.Equal(product.ProviderId, productDb.ProviderId);
        }
    }
}
