using TicTacToe.Model;

namespace TicTacToe.Logging
{
    interface ILogging
    {
        void ProcessLogMessage(LogInfo logInfo);
    }
}