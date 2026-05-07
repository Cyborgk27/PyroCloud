using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Shared.Infrastructure.Filters
{
    public class ResponseWrapperFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult obj && obj.Value is not ApiResponse<object>)
            {
                obj.Value = ApiResponse<object>.Ok(obj.Value);
            }
        }
    }
}
