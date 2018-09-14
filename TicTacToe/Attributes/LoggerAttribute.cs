using Microsoft.AspNetCore.Mvc.Filters;
using TicTacToe.Model;

namespace TicTacToe.Attributes
{
    public class LoggingAttribute : ResultFilterAttribute, IActionFilter, IExceptionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Exception == null)
            {
                Logging.Logging.Instance.ProcessLogMessage(new LogInfo(context.RouteData.Values["action"].ToString(), "Success", "No Exception", context.Result.ToString()));
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnException(ExceptionContext context)
        {
            Logging.Logging.Instance.ProcessLogMessage(new LogInfo(context.RouteData.Values["action"].ToString(), "Failure", context.Exception.Message.ToString(), context.Result.ToString()));    
        }
    }
}