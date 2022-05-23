using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using AutoMapper;

using Moq;

using ProductManagement.Interfaces;
using ProductManagement.Objects;
using ProductManagement.Services;

namespace ProductManagement.UnitTests.Services.ProductServiceTest
{
    [ExcludeFromCodeCoverage]
    public abstract class ProductServiceTestBase
    {
        protected static readonly Fixture fixture = new();

        protected readonly Mock<IMapper> _mapper = new();
        protected readonly Mock<IProductRepository> _repostory = new();
        protected readonly PaginationSettings _pagination = fixture.Create<PaginationSettings>();

        protected IProductService GetService(PaginationSettings pagination = default)
            => new ProductService(this._mapper.Object, this._repostory.Object, pagination ?? this._pagination);
    }
}
