using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

using Rxmxnx.PInvoke.Extensions;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBasicBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            this._service = service;
        }

        [HttpPost]
        public Task<ActionResult<Int32>> CreateAsync([FromBody] ProductSave product, CancellationToken cancellationToken)
            => base.ExecuteAsync(async () =>
            {
                Product result = await this._service.SaveProductAsync(product, cancellationToken);
                return result.Id;
            });

        [HttpGet]
        public async IAsyncEnumerable<ProductRead> GetCodesAsync([FromQuery] Int32? page, [FromQuery] ProductFilter filter, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            IMutableWrapper<Boolean> setHeaders = InputValue.CreateReference(true);
            IMutableWrapper<Int32> currentPageHeader = GetCurrentPage(page);
            IMutableWrapper<Int32> totalPageHeader = InputValue.CreateReference(0);
            await foreach (ProductRead productRead in this._service.GetProductsAsync(currentPageHeader, totalPageHeader, filter, cancellationToken)
                .WithCancellation(cancellationToken))
            {
                base.SetPaginationHeaders(setHeaders, currentPageHeader, totalPageHeader);
                yield return productRead;
            }
        }

        [HttpGet("{code}")]
        public Task<ActionResult<ProductRead>> GetProductAsync([FromRoute] Int32 code, CancellationToken cancellationToken)
            => base.ExecuteAsync(() => this._service.GetProductAsync(code, cancellationToken));

        [HttpPut("{code}")]
        public Task<ActionResult> UpdateProductAsync([FromRoute] Int32 code, [FromBody] ProductSave product, CancellationToken cancellationToken)
            => base.ExecuteAsync(() => this._service.SaveProductAsync(code, product, cancellationToken));
    }
}
