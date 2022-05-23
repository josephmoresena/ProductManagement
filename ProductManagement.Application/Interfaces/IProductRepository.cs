using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ProductManagement.Entities;
using ProductManagement.Objects;

namespace ProductManagement.Interfaces
{
    public interface IProductRepository
    {
        Task<Int32> CreateAsync(Product product, CancellationToken cancelationToken = default);
        Task UpdateAsync(Product product, CancellationToken cancelationToken = default);
        Task<Product> GetAsync(Int32 productId, CancellationToken cancelationToken = default);
        Task<TRead> GetAsync<TRead>(Int32 productId, CancellationToken cancelationToken = default) where TRead : class;
        Task<Int32> CountAsync(CancellationToken cancelationToken = default, ProductFilter filter = default);
        IAsyncEnumerable<TRead> GetAllAsync<TRead>(LimitSettings settings, CancellationToken cancelationToken = default, ProductFilter filter = default) where TRead : class;
    }
}
