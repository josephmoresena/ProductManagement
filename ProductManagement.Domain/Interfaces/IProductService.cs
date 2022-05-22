using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ProductManagement.Entities;

using Rxmxnx.PInvoke.Extensions;

namespace ProductManagement.Interfaces
{
    public interface IProductService<TRead, TWrite, TFilter>
        where TRead : class
        where TWrite : class
    {
        Task<TRead> GetProductAsync(Int32 productId, CancellationToken cancelationToken = default);
        Task<Product> SaveProductAsync(TWrite product, CancellationToken cancelationToken = default);
        Task SaveProductAsync(Int32 productId, TWrite product, CancellationToken cancelationToken = default);
        Task DeleteProductAsync(Int32 productId, CancellationToken cancelationToken = default);
        IAsyncEnumerable<TRead> GetProductsAsync(IMutableWrapper<Int32> page, IMutableWrapper<Int32> pageCount, TFilter filter, CancellationToken cancelationToken = default);
    }
}
