using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using Moq;

using ProductManagement.Entities;
using ProductManagement.Interfaces;

using Xunit;

namespace ProductManagement.UnitTests.Service.ProductServiceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class DeleteAsyncTest : ProductServiceTestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(false, true)]
        internal async Task TestAsync(Boolean noExists, Boolean isActive = false)
        {
            IProductService service = base.GetService();
            Int32 productId = fixture.Create<Int32>();
            Product dbValue = !noExists ? fixture.Build<Product>()
                .Without(p => p.Id).With(p => p.Active, isActive)
                .Create()
                : default;
            Times updateTimes = !noExists ? Times.Once() : Times.Never();

            base._repostory.Setup(r => r.GetAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(dbValue);
            if (!noExists)
            {
                await service.DeleteAsync(productId);
                Assert.False(dbValue.Active);
            }
            else
            {
                Exception ex = await Assert.ThrowsAnyAsync<Exception>(async () => await service.DeleteAsync(productId));
                Assert.Equal(typeof(ArgumentException), ex.GetType());
                Assert.Equal($"Invalid product id: {productId}", ex.Message);
            }

            base._repostory.Verify(r => r.GetAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
            base._repostory.Verify(r => r.UpdateAsync(dbValue, It.IsAny<CancellationToken>()), updateTimes);
        }
    }
}
