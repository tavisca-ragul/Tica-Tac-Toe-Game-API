namespace TicTacToe.Model
{
    public class LogInfo
    {
        public LogInfo(string request, string response, string exception, string comment)
        {
            Request = request;
            Response = response;
            Exception = exception;
            Comment = comment;
        }

        public string Request { get; }
        public string Response { get; }
        public string Exception { get; }
        public string Comment { get; }
    }
}