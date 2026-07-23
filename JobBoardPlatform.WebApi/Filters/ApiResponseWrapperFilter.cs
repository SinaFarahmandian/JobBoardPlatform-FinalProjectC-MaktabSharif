using JobBoardPlatform.Buisiness.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Filters;

public class ApiResponseWrapperFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.StatusCode is null or >= 200 and < 300)
        {
            var wrapperType = typeof(ApiResponse<>).MakeGenericType(objectResult.Value?.GetType() ?? typeof(object));
            var wrapped = Activator.CreateInstance(wrapperType)!;
            wrapperType.GetProperty("Success")!.SetValue(wrapped, true);
            wrapperType.GetProperty("Data")!.SetValue(wrapped, objectResult.Value);
            objectResult.Value = wrapped;
        }
        else if (context.Result is StatusCodeResult statusResult && statusResult.StatusCode is >= 200 and < 300)
        {
            context.Result = new ObjectResult(ApiResponse<object>.SuccessResponse(new { })) { StatusCode = statusResult.StatusCode };
        }

        await next();
    }
}