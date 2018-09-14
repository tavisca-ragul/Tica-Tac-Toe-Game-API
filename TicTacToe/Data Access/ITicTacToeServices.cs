using TicTacToe.Model;

namespace TicTacToe.Data_Access
{
    interface ITicTacToeServices
    {
        ITicTacToeServices GetServices();
        int RegisterUser(User user);
        string GetUserNameByID(int id);
        bool CheckUserName(string username);
        string GenerateAPIKey(string username);
        bool Authorize(string apiKey);
        int GameStart(string playerOneAPIKey, string playerTwoAPIKey);
        bool IsGameID(int id);
        bool CheckPlayerInGameByID(string apiKey, int gameID);
        bool ChechPlayerMoveInGameByID(string apiKey, int gameID);
        bool CheckMoveIsAvailableInGame(int gameID, int move);
        void MakeAMove(int gameID, int move, string apiKey);
        string GetStatus(int gameID);
        bool CheckStatus(int gameID);
    }
}