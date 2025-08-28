using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace interview.Filters
{
    public class ErrorFilter : IExceptionFilter
    {
        private readonly ILogger<ErrorFilter> _logger;

        public ErrorFilter(ILogger<ErrorFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // 紀錄錯誤
            _logger.LogError(context.Exception, "Unhandled exception occurred.");

            // 判斷是否為 API (JSON) 還是 MVC (View)
            if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                context.Result = new JsonResult(new
                {
                    success = false,
                    message = "An unexpected error occurred. Please try again later."
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            else
            {
                // 導向 Error Page
                context.Result = new RedirectToActionResult("Error", "Home", null);
            }

            context.ExceptionHandled = true; // 標記為已處理
        }
    }
}
