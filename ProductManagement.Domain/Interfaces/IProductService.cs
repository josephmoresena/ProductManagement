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
        Task<TRead> GetAsync(Int32 productId, CancellationToken cancelationToken = default);
        Task<Product> SaveAsync(TWrite product, CancellationToken cancelationToken = default);
        Task SaveAsync(Int32 productId, TWrite product, CancellationToken cancelationToken = default);
        Task DeleteAsync(Int32 productId, CancellationToken cancelationToken = default);
        IAsyncEnumerable<TRead> GetAllAsync(IMutableWrapper<Int32> page, IMutableWrapper<Int32> pageCount, TFilter filter, CancellationToken cancelationToken = default);
    }
}
