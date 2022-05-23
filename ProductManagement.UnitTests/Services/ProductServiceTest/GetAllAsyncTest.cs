using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using Moq;

using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Rxmxnx.PInvoke.Extensions;

using Xunit;

namespace ProductManagement.UnitTests.Services.ProductServiceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class GetAllAsyncTest : ProductServiceTestBase
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        internal async Task TestAsync(Int32 page)
        {
            Int32 limit = 5;
            Int32 total = limit * limit - 2;
            IProductService service = base.GetService(new()
            {
                RowsByPage = limit,
            });
            ProductFilter filter = new();
            ISet<ProductRead> result = fixture.CreateMany<ProductRead>(total).ToHashSet();
            IMutableWrapper<Int32> currentPageHeader = InputValue.CreateReference(page);
            IMutableWrapper<Int32> totalPageHeader = InputValue.CreateReference(0);

            Int32 totalCount = (Int32)Math.Ceiling(((Double)total) / limit);
            Int32 realPage = page < 1 ? 1 : page <= totalCount ? page : totalCount;

            base._repostory.Setup(r => r.CountAsync(It.IsAny<CancellationToken>(), filter)).ReturnsAsync(total);
            base._repostory.Setup(r => r.GetAllAsync<ProductRead>(It.IsAny<LimitSettings>(), It.IsAny<CancellationToken>(), filter))
                .Returns(ConvertToAsyncEnumerable(result.Skip((realPage - 1) * limit).Take(limit)))
                .Callback<LimitSettings, CancellationToken, ProductFilter>((l, c, f) =>
                {
                    Assert.InRange(l.Page, 1, limit);
                    Assert.Equal(l.Limit, limit);
                    Assert.Equal(realPage, l.Page);
                });

            Int32 count = 0;
            await foreach (ProductRead product in service.GetAllAsync(currentPageHeader, totalPageHeader, new()))
            {
                Assert.Contains(product, result);
                count++;
            }

            Assert.Equal(realPage, currentPageHeader.Value);
            Assert.Equal(totalCount, totalPageHeader.Value);
            Assert.Equal(realPage < totalCount ? limit : total % limit, count);
        }

        private static async IAsyncEnumerable<TResult> ConvertToAsyncEnumerable<TResult>(IEnumerable<TResult> enumeration)
        {
            foreach (TResult item in await Task.FromResult(enumeration))
                yield return item;
        }
    }
}
