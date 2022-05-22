using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

namespace ProductManagement.Services
{
    public sealed class ProductProviderService : IProductProviderService
    {
        private readonly IMapper _mapper;
        private readonly IProductProviderRepostory _repostory;

        public ProductProviderService(IMapper mapper, IProductProviderRepostory repostory)
        {
            this._mapper = mapper;
            this._repostory = repostory;
        }

        IAsyncEnumerable<Int32> IProductProviderService<ProviderSave>.GetProviderCodes(CancellationToken cancelationToken)
            => this._repostory.GetCodesAsync(cancelationToken);

        async Task<ProductProvider> IProductProviderService<ProviderSave>.SaveProviderAsync(ProviderSave provider, CancellationToken cancelationToken)
        {
            ProductProvider product = this._mapper.Map<ProductProvider>(provider);
            await this._repostory.CreateAsync(product, cancelationToken);
            return product;
        }
    }
}
