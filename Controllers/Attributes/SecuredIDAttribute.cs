using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Attributes
{
    public class SecuredIDAttribute : Attribute, IActionFilter
    {
        private MyContext _context = new MyContext();

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            TokensService service = new TokensService();
            
            ControllerBase controller = context.Controller as ControllerBase;
            string header = controller.Request.Headers["Authorization"];

            int? id = service.GetUserIDFromToken(header);

            if (id == null)
            {
                context.Result = controller.Unauthorized(new { message = "Can't get id from token" });
                return;
            }

            if (context.ActionArguments.TryGetValue("id", out var actionUserID) && actionUserID is int)
            {
                if ((int)actionUserID != id)
                {
                    context.Result = controller.StatusCode(403, new { message = "User doesn't have permission" });
                }
            }
            else
            {
                context.Result = controller.Unauthorized(new { message = "Can't get id parameter for authorization" });
            }
        }
    }
}
