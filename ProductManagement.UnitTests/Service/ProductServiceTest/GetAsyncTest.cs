using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using Moq;

using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Service.ProductServiceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class GetAsyncTest : ProductServiceTestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task TestAsync(Boolean exist)
        {
            IProductService service = base.GetService();
            ProductRead result = exist ? fixture.Create<ProductRead>() : default;
            Int32 productId = (result?.Code).GetValueOrDefault();
            base._repostory.Setup(r => r.GetAsync<ProductRead>(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            if (exist)
                Assert.Equal(result, await service.GetAsync(productId));
            else
            {
                Exception ex = await Assert.ThrowsAnyAsync<Exception>(async () => await service.GetAsync(productId));
                Assert.Equal(typeof(ArgumentException), ex.GetType());
                Assert.Equal($"Invalid product id: {productId}", ex.Message);
            }

            base._repostory.Verify(r => r.GetAsync<ProductRead>(productId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
