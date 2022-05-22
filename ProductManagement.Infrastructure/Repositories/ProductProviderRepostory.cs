using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ProductManagement.Entities;
using ProductManagement.Interfaces;

namespace ProductManagement.Infrastructure.Repositories
{
    public sealed class ProductProviderRepostory : IProductProviderRepostory
    {
        private readonly ProductManagementDbContext _dbContext;

        public ProductProviderRepostory(ProductManagementDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        async Task IProductProviderRepostory.CreateAsync(ProductProvider provider, CancellationToken cancelationToken)
        {
            await this._dbContext.AddAsync(provider, cancelationToken);
            await this._dbContext.SaveChangesAsync(cancelationToken);
        }

        async IAsyncEnumerable<Int32> IProductProviderRepostory.GetCodesAsync([EnumeratorCancellation] CancellationToken cancelationToken)
        {
            await foreach (Int32 providerId in this._dbContext.Providers
                .OrderBy(p => p.Id).Select(t => t.Id)
                .AsAsyncEnumerable().WithCancellation(cancelationToken))
                yield return providerId;
        }
    }
}
