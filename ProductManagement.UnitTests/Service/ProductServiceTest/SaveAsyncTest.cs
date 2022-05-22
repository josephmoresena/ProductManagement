using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using Moq;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Service.ProductServiceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class SaveAsyncTest : ProductServiceTestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(false, true)]
        [InlineData(false, null)]
        internal async Task CreateTestAsync(Boolean noInput, Boolean? dateError = false)
        {
            IProductService service = base.GetService();
            DateTime manufacturingDate = DateTime.Now.AddYears(-1).Date;
            DateTime expirationDate = dateError.HasValue ?
                dateError.Value ? manufacturingDate.AddDays(-1) : DateTime.Now.Date
                : manufacturingDate;
            ProductSave save = !noInput ? fixture.Create<ProductSave>() : default;
            Int32 productId = fixture.Create<Int32>();
            Product result = fixture.Build<Product>()
                .With(p => p.ManufacturingDate, manufacturingDate)
                .With(p => p.ExpirationDate, expirationDate)
                .Without(p => p.Id).Create();
            Times timesMapper = !noInput ? Times.Once() : Times.Never();
            Times timesCreate = dateError.HasValue && !dateError.Value ? timesMapper : Times.Never();

            base._mapper.Setup(m => m.Map<Product>(save)).Returns(result);
            base._repostory.Setup(r => r.CreateAsync(result, It.IsAny<CancellationToken>())).ReturnsAsync(productId);

            if (!noInput && dateError.HasValue && !dateError.Value)
            {
                Assert.Equal(default, result.Id);
                Assert.Equal(result, await service.SaveAsync(save));
                Assert.True(result.Id != default);
                Assert.Equal(productId, result.Id);
            }
            else
            {
                Exception ex = await Assert.ThrowsAnyAsync<Exception>(async () => await service.SaveAsync(save));
                if (noInput)
                    Assert.Equal(typeof(ArgumentNullException), ex.GetType());
                else
                {
                    Assert.Equal(typeof(ArgumentException), ex.GetType());
                    Assert.Equal("Product manufacturing date must be lower than expiration date.", ex.Message);
                }
            }

            base._mapper.Verify(m => m.Map<Product>(save), timesMapper);
            base._repostory.Verify(r => r.CreateAsync(result, It.IsAny<CancellationToken>()), timesCreate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(false, true)]
        [InlineData(false, false, null)]
        [InlineData(false, false, true)]
        internal async Task UpdateTestAsync(Boolean noExists, Boolean noInput = false, Boolean? dateError = false)
        {
            IProductService service = base.GetService();
            DateTime manufacturingDate = DateTime.Now.AddYears(-1).Date;
            DateTime expirationDate = dateError.HasValue ?
                dateError.Value ? manufacturingDate.AddDays(-1) : DateTime.Now.Date
                : manufacturingDate;
            Int32 productId = fixture.Create<Int32>();
            Product dbValue = !noExists ? fixture.Build<Product>()
                .With(p => p.ManufacturingDate, manufacturingDate)
                .With(p => p.ExpirationDate, expirationDate)
                .With(p => p.Id, productId).Create()
                : default;
            ProductSave save = !noInput ? fixture.Create<ProductSave>() : default;

            Times getTimes = Times.Once();
            Times timesMapper = !noExists && !noInput ? getTimes : Times.Never();
            Times timesCreate = dateError.HasValue && !dateError.Value ? timesMapper : Times.Never();

            base._repostory.Setup(r => r.GetAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(dbValue);
            base._mapper.Setup(m => m.Map(save, dbValue)).Returns(dbValue);

            if (!noExists && !noInput && dateError.HasValue && !dateError.Value)
                await service.SaveAsync(productId, save);
            else
            {
                Exception ex = await Assert.ThrowsAnyAsync<Exception>(async () => await service.SaveAsync(productId, save));
                if (noExists)
                {
                    Assert.Equal(typeof(ArgumentException), ex.GetType());
                    Assert.Equal($"Invalid product id: {productId}", ex.Message);
                }
                else if (noInput)
                    Assert.Equal(typeof(ArgumentNullException), ex.GetType());
                else
                {
                    Assert.Equal(typeof(ArgumentException), ex.GetType());
                    Assert.Equal("Product manufacturing date must be lower than expiration date.", ex.Message);
                }
            }

            base._repostory.Verify(r => r.GetAsync(productId, It.IsAny<CancellationToken>()), getTimes);
            base._mapper.Verify(m => m.Map(save, dbValue), timesMapper);
            base._repostory.Verify(r => r.UpdateAsync(dbValue, It.IsAny<CancellationToken>()), timesCreate);
        }
    }
}
