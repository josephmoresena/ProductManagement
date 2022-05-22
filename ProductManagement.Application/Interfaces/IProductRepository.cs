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
        Task<Int32> CreateAsync(Product product, CancellationToken cancelationToken);
        Task UpdateAsync(Product product, CancellationToken cancelationToken);
        Task<Product> GetAsync(Int32 productId, CancellationToken cancelationToken);
        Task<TDto> GetAsync<TDto>(Int32 productId, CancellationToken cancelationToken) where TDto : class;
        Task<Int32> CountAsync(CancellationToken cancelationToken, ProductFilter filter);
        IAsyncEnumerable<TDto> GetAllAsync<TDto>(LimitSettings settings, CancellationToken cancelationToken, ProductFilter filter = default) where TDto : class;
    }
}
