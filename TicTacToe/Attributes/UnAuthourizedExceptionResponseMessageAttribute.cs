using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TicTacToe.Attributes
{
    public class UnAuthourizedExceptionResponseMessageAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled && context.Exception is UnauthorizedAccessException)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(context.Exception.Message);
                context.ExceptionHandled = true;
            }
        }
    }
}