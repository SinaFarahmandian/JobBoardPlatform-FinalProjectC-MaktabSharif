using JobBoardPlatform.Buisiness.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.MVC.Filters;

public class MvcExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var (statusCode, message) = context.Exception switch
        {
            NotFoundException => (404, context.Exception.Message),
            ForbiddenAccessException => (403, context.Exception.Message),
            BadRequestException => (400, context.Exception.Message),
            InvalidStatusTransitionException => (400, context.Exception.Message),
            _ => (500, "خطای غیرمنتظره‌ای رخ داد")
        };

        context.Result = new ViewResult
        {
            ViewName = "Error",
            ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                context.ModelState)
            {
                ["Message"] = message,
                ["StatusCode"] = statusCode
            }
        };
        context.HttpContext.Response.StatusCode = statusCode;
        context.ExceptionHandled = true;
    }
}