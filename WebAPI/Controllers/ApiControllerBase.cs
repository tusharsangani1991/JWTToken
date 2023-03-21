using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Controllers
{
    
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ApiExceptionFilterAttribute))]
    public class ApiControllerBase : ControllerBase, IActionFilter
    {

        public ApiControllerBase()
        {

        }
        protected IToken Token;
        protected Guid GroupId;
        protected Guid UserId;
        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            var (apiToken, token) = ApiAuthToken.From(User);
            context.HttpContext.Items["_apiAuthToken"] = apiToken; //Used for logging
            if (apiToken != null && token != null)
            {
                Token = token;
                GroupId = apiToken.GroupId;                
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context) { }
    }
}
