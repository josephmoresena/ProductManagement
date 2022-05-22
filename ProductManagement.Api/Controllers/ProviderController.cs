using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ProductManagement.Entities;
using ProductManagement.Interfaces;
using ProductManagement.Objects;

namespace ProductManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProviderController : ControllerBasicBase
    {
        private readonly IProductProviderService _service;

        public ProviderController(IProductProviderService service)
        {
            this._service = service;
        }

        [HttpPost]
        public Task<ActionResult<Int32>> CreateAsync([FromBody] ProviderSave provider, CancellationToken cancellationToken)
            => base.ExecuteAsync(async () =>
            {
                ProductProvider result = await this._service.SaveProviderAsync(provider, cancellationToken);
                return result.Id;
            });

        [HttpGet("Codes")]
        public IAsyncEnumerable<Int32> GetCodesAsync(CancellationToken cancellationToken)
            => this._service.GetProviderCodes(cancellationToken);
    }
}
