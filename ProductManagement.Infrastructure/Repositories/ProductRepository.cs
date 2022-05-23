using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

namespace ProductManagement.Infrastructure.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ProductManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(ProductManagementDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        Task<Int32> IProductRepository.CountAsync(CancellationToken cancelationToken, ProductFilter filter)
            => this._dbContext.Products.Filter(filter).CountAsync(cancelationToken);

        async Task<Int32> IProductRepository.CreateAsync(Product product, CancellationToken cancelationToken)
        {
            await this._dbContext.AddAsync(product, cancelationToken);
            await this._dbContext.SaveChangesAsync(cancelationToken);
            return product.Id;
        }

        async IAsyncEnumerable<TRead> IProductRepository.GetAllAsync<TRead>(LimitSettings settings, [EnumeratorCancellation] CancellationToken cancelationToken, ProductFilter filter)
        {
            await foreach (TRead read in this._mapper.ProjectTo<TRead>(this._dbContext.Products
                .Filter(filter).OrderBy(p => p.Id).Limit(settings))
                .AsAsyncEnumerable().WithCancellation(cancelationToken))
                yield return read;
        }

        Task<Product> IProductRepository.GetAsync(Int32 productId, CancellationToken cancelationToken)
            => this._dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id.Equals(productId), cancelationToken);

        Task<TRead> IProductRepository.GetAsync<TRead>(Int32 productId, CancellationToken cancelationToken)
            => this._mapper.ProjectTo<TRead>(this._dbContext.Products.Where(p => p.Id.Equals(productId)))
            .FirstOrDefaultAsync();

        async Task IProductRepository.UpdateAsync(Product product, CancellationToken cancelationToken)
        {
            Product productDb = await this._dbContext.Products.SingleAsync(p => p.Id.Equals(product.Id));
            this._dbContext.Entry(product).CurrentValues.SetValues(productDb);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
