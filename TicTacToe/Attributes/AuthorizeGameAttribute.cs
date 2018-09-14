using Microsoft.AspNetCore.Mvc.Filters;
using System;
using TicTacToe.Data_Access;

namespace TicTacToe.Attributes
{
    public class AuthorizeGameAttribute : ResultFilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ITicTacToeServices services = new TicTacToeSQLServices();
            var playerOneAPIKey = context.HttpContext.Request.Headers["playerOneAPIKey"].ToString();
            if (string.IsNullOrEmpty(playerOneAPIKey))
                throw new UnauthorizedAccessException("Player one API key not found");
            if (!services.Authorize(playerOneAPIKey))
                throw new UnauthorizedAccessException("Player one API key is invalid");
            var playerTwoAPIKey = context.HttpContext.Request.Headers["playerTwoAPIKey"].ToString();
            if (string.IsNullOrEmpty(playerTwoAPIKey))
                throw new UnauthorizedAccessException("Player two API key not found");
            if (!services.Authorize(playerTwoAPIKey))
                throw new UnauthorizedAccessException("Player two API key is invalid");
        }
    }
}