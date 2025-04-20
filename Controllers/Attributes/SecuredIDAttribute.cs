using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Attributes
{
    public class SecuredIDAttribute : Attribute, IActionFilter
    {
        private int _id;
        private MyContext _context = new MyContext();
        
        public SecuredIDAttribute(int id)
        {
            _id = id;
        }

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
                context.Result = controller.Unauthorized(new { message = "Authorization failed" });
            }
            if (!_context.UserRights.Any(Right => Right.UserID == id))
            {
                context.Result = controller.Unauthorized(new { message = "User doesn't have permission" });
            }
        }
    }
}
