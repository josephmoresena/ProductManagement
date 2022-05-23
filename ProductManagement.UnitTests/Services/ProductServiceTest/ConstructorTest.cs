using System;
using System.Diagnostics.CodeAnalysis;

using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Services.ProductServiceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ConstructorTest : ProductServiceTestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void ExceptionTest(Boolean negative)
        {
            PaginationSettings pagination = new() { RowsByPage = !negative ? 0 : -1, };
            Exception ex = Assert.ThrowsAny<Exception>(() => this.GetService(pagination));
            Assert.Equal(typeof(ArgumentException), ex.GetType());
            Assert.Equal($"Invalid pagination value ({pagination.RowsByPage}).", ex.Message);
        }
    }
}
