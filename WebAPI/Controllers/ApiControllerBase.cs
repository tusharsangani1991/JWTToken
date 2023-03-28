using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Controllers
{

    [ApiController]
    //[Authorize]
    [TypeFilter(typeof(ApiExceptionFilterAttribute))]
    public class ApiControllerBase : ControllerBase, IActionFilter
    {

        public ApiControllerBase()
        {

        }
        protected IToken Token;        
        protected Guid UserId;

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            
            if (!context.Filters.Any(v => v is AllowAnonymousFilter))
            {
            }
            var (apiToken, token) = ApiAuthToken.From(User);
            context.HttpContext.Items["_apiAuthToken"] = apiToken; //Used for logging
            if (apiToken != null && token != null)
            {
                Token = token;
               // GroupId = apiToken.GroupId;
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context) { }
    }
}
