using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TicTacToe.Data_Access;

namespace TicTacToe.Attributes
{
    public class AuthorizeMoveAttribute : ResultFilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ITicTacToeServices services = new TicTacToeSQLServices();
            var apiKey = context.HttpContext.Request.Headers["apikey"].ToString();
            if (string.IsNullOrEmpty(apiKey))
                throw new UnauthorizedAccessException("API key not found");
            if(!services.Authorize(apiKey))
                throw new UnauthorizedAccessException("API key is invalid");
        }
    }
}