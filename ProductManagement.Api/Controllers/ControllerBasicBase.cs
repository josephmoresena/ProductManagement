using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Rxmxnx.PInvoke.Extensions;

namespace ProductManagement.Controllers
{
    public class ControllerBasicBase : ControllerBase
    {
        protected void SetPaginationHeaders(IMutableWrapper<Boolean> setHeaders, IMutableWrapper<Int32> currentPageHeader, IMutableWrapper<Int32> totalPageHeader)
        {
            if (setHeaders.Value)
            {
                this.HttpContext.Response.Headers.Add("totalPages", $"{totalPageHeader.Value}");
                this.HttpContext.Response.Headers.Add("page", $"{currentPageHeader.Value}");
                setHeaders.SetInstance(false);
            }
        }
        protected async Task<ActionResult> ExecuteAsync(Func<Task> taskFunc)
        {
            try
            {
                await taskFunc();
                return this.NoContent();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is ArgumentNullException)
                    return BadRequest(ex.Message);
                else
                    throw;
            }
        }
        protected async Task<ActionResult<TResult>> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc)
        {
            try
            {
                return this.Ok(await taskFunc());
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is ArgumentNullException)
                    return BadRequest(ex.Message);
                else
                    throw;
            }
        }

        protected static IMutableWrapper<Int32> GetCurrentPage(Int32? page)
            => InputValue.CreateReference(page.GetValueOrDefault());
    }
}
