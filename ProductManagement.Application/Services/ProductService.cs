using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Rxmxnx.PInvoke.Extensions;

namespace ProductManagement.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repostory;
        private readonly PaginationSettings _pagination;

        public ProductService(IMapper mapper, IProductRepository repostory, PaginationSettings pagination)
        {
            this._mapper = mapper;
            this._repostory = repostory;
            this._pagination = pagination;

            if (this._pagination.RowsByPage == default)
                throw new ArgumentException("Invalid pagination value.");
        }

        async Task IProductService<ProductRead, ProductSave, ProductFilter>.DeleteProductAsync(Int32 productId, CancellationToken cancelationToken)
        {
            Product product = await this._repostory.GetAsync(productId, cancelationToken) ?? throw GetInvalidProductException(productId);
            product.Active = false;
            await this._repostory.UpdateAsync(product, cancelationToken);
        }


        async Task<ProductRead> IProductService<ProductRead, ProductSave, ProductFilter>.GetProductAsync(Int32 productId, CancellationToken cancelationToken)
            => await this._repostory.GetAsync<ProductRead>(productId, cancelationToken) ?? throw GetInvalidProductException(productId);

        async IAsyncEnumerable<ProductRead> IProductService<ProductRead, ProductSave, ProductFilter>.GetProductsAsync(IMutableWrapper<Int32> page, IMutableWrapper<Int32> pageCount, ProductFilter filter, [EnumeratorCancellation] CancellationToken cancelationToken)
        {
            LimitSettings settings = await GetLimitSettingAsync(page, pageCount, cancelationToken, filter);
            await foreach (ProductRead product in this._repostory.GetAllAsync<ProductRead>(settings, cancelationToken, filter)
                .WithCancellation(cancelationToken))
                yield return product;
        }

        async Task<Product> IProductService<ProductRead, ProductSave, ProductFilter>.SaveProductAsync(ProductSave product, CancellationToken cancelationToken)
        {
            Product productDb = this._mapper.Map<Product>(product);
            ValidateProduct(productDb);
            productDb.Id = await this._repostory.CreateAsync(productDb, cancelationToken);
            return productDb;
        }

        async Task IProductService<ProductRead, ProductSave, ProductFilter>.SaveProductAsync(Int32 productId, ProductSave product, CancellationToken cancelationToken)
        {
            Product productDb = await this._repostory.GetAsync(productId, cancelationToken) ?? throw GetInvalidProductException(productId);
            ValidateProduct(productDb);
            this._mapper.Map(product, productDb);
            await this._repostory.UpdateAsync(productDb, cancelationToken);
        }

        private async Task<LimitSettings> GetLimitSettingAsync(IMutableWrapper<Int32> page, IMutableWrapper<Int32> pageCount, CancellationToken cancelationToken, ProductFilter filter)
        {
            Double total = await this._repostory.CountAsync(cancelationToken, filter);
            pageCount.SetInstance((Int32)Math.Ceiling(total / this._pagination.RowsByPage));

            if (page.Value <= 0)
                page.SetInstance(1);

            if (page.Value > pageCount.Value)
                page.SetInstance(pageCount.Value);

            LimitSettings settings = new()
            {
                Page = page.Value,
                Limit = this._pagination.RowsByPage,
            };
            return settings;
        }

        private static void ValidateProduct(Product productDb)
        {
            if (productDb.ManufacturingDate >= productDb.ExpirationDate)
                throw new ArgumentException("Product manufacturing date must be lower than expiration date.");
        }

        private static Exception GetInvalidProductException(Int32 productId)
            => new ArgumentNullException($"Invalid product id: {productId}");
    }
}
