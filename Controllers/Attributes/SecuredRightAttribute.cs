using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Enums;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Attributes
{
    public class SecuredRightAttribute : Attribute, IActionFilter
    {
        private UserRightType _right;
        private MyContext _context = new MyContext();
        
        public SecuredRightAttribute(UserRightType rightType)
        {
            _right = rightType;
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
            if (!_context.UserRights.Any(Right => Right.UserID == id && Right.Right == _right.ToString()))
            {
                context.Result = controller.Unauthorized(new { message = "User doesn't have permission" });
            }
        }
    }
}
