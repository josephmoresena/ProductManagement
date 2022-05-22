using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ProductManagement.Entities;

namespace ProductManagement.Interfaces
{
    public interface IProductProviderService<TWrite>
        where TWrite : class
    {
        Task<ProductProvider> SaveProviderAsync(TWrite provider, CancellationToken cancelationToken);
        IAsyncEnumerable<Int32> GetProviderCodes(CancellationToken cancelationToken);
    }
}
