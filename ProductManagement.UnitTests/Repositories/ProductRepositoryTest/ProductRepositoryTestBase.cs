using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AutoFixture;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Entities;
using ProductManagement.Infrastructure;
using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

namespace ProductManagement.UnitTests.Repositories.ProductRepositoryTest
{
    [ExcludeFromCodeCoverage]
    public abstract class ProductRepositoryTestBase
    {
        protected static readonly Fixture fixture = new();
        protected static readonly IMapper mapper;

        static ProductRepositoryTestBase()
        {
            MapperConfiguration mapperConfiguration = new(c => c.AddProfile<ProductManagementProfile>());
            mapperConfiguration.CompileMappings();
            mapper = mapperConfiguration.CreateMapper();
        }

        protected IProductRepository GetRepository(ProductManagementDbContext dbContext) => new ProductRepository(dbContext, mapper);

        protected static ILocalDbContext CreateContext(Boolean withData) => new LocalDbContext(withData);
        protected static IQueryable<Product> Filter(ProductFilter filter, IQueryable<Product> source)
        {
            IQueryable<Product> result = source;
            if (filter.Products.Any())
                result = result.Where(p => filter.Products.Contains(p.Id));

            if (filter.Providers.Any())
                result = result.Where(p => filter.Providers.Contains(p.Provider.Id));

            if (!String.IsNullOrEmpty(filter.ProductDescription))
                if (filter.ExactMatchProductDescription)
                    result = result.Where(p => p.Description.Equals(filter.ProductDescription));
                else
                    result = result.Where(p => p.Description.Contains(filter.ProductDescription));

            if (!String.IsNullOrEmpty(filter.ProviderDescription))
                if (filter.ExactMatchProviderDescription)
                    result = result.Where(p => p.Provider.Description.Equals(filter.ProviderDescription));
                else
                    result = result.Where(p => p.Provider.Description.Contains(filter.ProviderDescription));

            if (filter.Status.HasValue)
                result = result.Where(p => p.Active.Equals(filter.Status.Equals(ProductStatus.Active)));

            return result;
        }

        protected interface ILocalDbContext : IDisposable
        {
            IQueryable<Product> Products { get; }
            IQueryable<ProductProvider> Providers { get; }
            ProductManagementDbContext Context { get; }
        }
        private sealed record LocalDbContext : ILocalDbContext
        {
            private readonly Guid _key;
            private readonly ProductManagementDbContext _dbContext;
            private Boolean _disposed = false;

            public Guid Key => this._key;
            public ProductManagementDbContext Context => this._dbContext;
            public IQueryable<Product> Products => this._dbContext.Products.OrderBy(p => Guid.NewGuid()).AsNoTracking();
            public IQueryable<ProductProvider> Providers => this._dbContext.Providers.OrderBy(p => Guid.NewGuid()).AsNoTracking();

            /// <summary>
            /// Constructor.
            /// </summary>
            public LocalDbContext(Boolean withData)
            {
                this._key = Guid.NewGuid();

                DbContextOptionsBuilder builder = new DbContextOptionsBuilder<ProductManagementDbContext>()
                    .UseInMemoryDatabase(databaseName: $"{typeof(LocalDbContext).Name}{this._key}");
                DbContextOptions dbContextOptions = builder.Options;
                this._dbContext = new(dbContextOptions);
                this._dbContext.Database.EnsureDeleted();
                this._dbContext.Database.EnsureCreated();

                this._dbContext.Add(fixture.Build<ProductProvider>().Without(p => p.Id).Create());

                ProductProvider[] providers = fixture.Build<ProductProvider>().Without(p => p.Id)
                    .CreateMany(withData ? 10 : 1).ToArray();

                if (withData)
                    foreach (ProductProvider provider in providers)
                        provider.Products.AddMany(() => fixture.Build<Product>()
                        .Without(p => p.Id).Without(p => p.ProviderId).Create(), 10);

                this._dbContext.AddRange(providers);
                this._dbContext.SaveChanges();
            }

            /// <summary>
            /// Dispose method.
            /// </summary>
            public void Dispose()
            {
                if (!this._disposed)
                {
                    this._disposed = true;
                    this._dbContext.Database.EnsureDeleted();
                    this._dbContext.Dispose();
                }
            }
        }
    }
}
