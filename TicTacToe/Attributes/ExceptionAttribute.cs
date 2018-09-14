using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicTacToe.Attributes
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(!context.ExceptionHandled)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new JsonResult("Internal Server Error");
                context.ExceptionHandled = true;
            }
        }
    }
}