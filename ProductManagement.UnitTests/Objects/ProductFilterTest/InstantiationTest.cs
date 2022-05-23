using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using ProductManagement.Objects;

using Xunit;

namespace ProductManagement.UnitTests.Objects.ProductFilterTest
{
    [ExcludeFromCodeCoverage]
    public sealed class InstantiationTest
    {
        private static readonly Fixture fixture = new();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void Test(Boolean nullArray)
        {
            ProductFilter filter = new()
            {
                ExactMatchProductDescription = fixture.Create<Boolean>(),
                ExactMatchProviderDescription = fixture.Create<Boolean>(),
                ProductDescription = fixture.Create<String>(),
                ProviderDescription = fixture.Create<String>(),
                Status = fixture.Create<ProductStatus?>(),
                Products = !nullArray ? fixture.Create<Int32[]>() : default,
                Providers = !nullArray ? fixture.Create<Int32[]>() : default,
            };

            Assert.NotNull(filter.Providers);
            Assert.NotNull(filter.Products);

            if (nullArray)
            {
                Assert.Empty(filter.Providers);
                Assert.Empty(filter.Products);
            }
        }
    }
}
