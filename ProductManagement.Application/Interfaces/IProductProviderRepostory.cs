using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ProductManagement.Entities;

namespace ProductManagement.Interfaces
{
    public interface IProductProviderRepostory
    {
        Task CreateAsync(ProductProvider provider, CancellationToken cancelationToken);
        IAsyncEnumerable<Int32> GetCodesAsync(CancellationToken cancelationToken);
    }
}
