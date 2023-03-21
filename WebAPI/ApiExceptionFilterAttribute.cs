using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ApiExceptionFilterAttribute(ILoggerFactory logFact, IWebHostEnvironment hostEnv)
        {
            m_logger = logFact.CreateLogger(GetType());
            m_hostEnv = hostEnv;
        }

        protected readonly ILogger m_logger;
        protected readonly IWebHostEnvironment m_hostEnv;

        public override void OnException(ExceptionContext context)
        {
            m_logger.LogError(context.Exception, "An unhandled exception occured");
            if (m_hostEnv.IsDevelopment())
                context.Result = new ObjectResult(new { success = false, error = context.Exception.ToString() }) { StatusCode = 500 };
            else
                context.Result = new ObjectResult(new { success = false, error = "An unexpected error occured" }) { StatusCode = 500 };
            context.ExceptionHandled = true;
        }
    }
}
