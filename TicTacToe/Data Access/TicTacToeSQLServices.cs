using System;
using System.Data;
using System.Data.SqlClient;
using TicTacToe.Model;

namespace TicTacToe.Data_Access
{
    class TicTacToeSQLServices : ITicTacToeServices
    {
        SqlConnectionStringBuilder Builder;
        SqlConnection Connection;
        SqlCommand Statement;
        String Query;
        public TicTacToeSQLServices()
        {
            Builder = new SqlConnectionStringBuilder();
            Builder.DataSource = "TAVDESK136";
            Builder.UserID = "sa";
            Builder.Password = "test123!@#";
            Builder.InitialCatalog = "TicTacToe";          
            Query = null;
            Statement = null;
        }

        public ITicTacToeServices GetServices()
        {
            return new TicTacToeSQLServices();
        }

        public int RegisterUser(User user)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "insert into user_details(first_name, last_name, user_name) values(@FirstName, @LastName, @UserName)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@FirstName", user.FirstName);
                    Statement.Parameters.AddWithValue("@LastName", user.LastName);
                    Statement.Parameters.AddWithValue("@UserName", user.UserName);
                    Statement.ExecuteNonQuery();
                }
                Query = "select max(id) from  user_details";
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    SqlDataReader ResultSet = Statement.ExecuteReader();
                    if (ResultSet.Read())
                        return ResultSet.GetInt32(0);
                    throw new Exception();
                }
            }
        }

        public string GetUserNameByID(int id)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select user_name from user_details where id = (@ID)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@ID", id);
                    SqlDataReader ResultSet = Statement.ExecuteReader();
                    if (ResultSet.Read())
                        return ResultSet.GetString(0);
                    return string.Empty;
                }
            }
        }

        public bool CheckUserName(string username)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select * from user_details where user_name = (@UserName)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@UserName", username);
                    SqlDataReader ResultSet = Statement.ExecuteReader();
                    if (ResultSet.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public string GenerateAPIKey(string username)
        {
            string apiKey = $"{username}{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond}";
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "update user_details set api_key = (@APIKey) where user_name = (@UserName)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@APIKey", apiKey);
                    Statement.Parameters.AddWithValue("@UserName", username);
                    Statement.ExecuteNonQuery();
                    return apiKey;
                }
            }
        }

        public bool Authorize(string apiKey)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select * from user_details where api_key = (@APIKey)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@APIKey", apiKey);
                    SqlDataReader ResultSet = Statement.ExecuteReader();
                    if (ResultSet.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public int GameStart(string playerOneAPIKey, string playerTwoAPIKey)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "insert into game_info (player_one_api_key, player_two_api_key, moves, status) values (@PlayerOneAPIKey, @PlayerTwoAPIKey, @Moves, @Status)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@PlayerOneAPIKey", playerOneAPIKey);
                    Statement.Parameters.AddWithValue("@PlayerTwoAPIKey", playerTwoAPIKey);
                    Statement.Parameters.AddWithValue("@Moves", 0);
                    Statement.Parameters.AddWithValue("@Status", "In Progress");
                    Statement.ExecuteNonQuery();
                }
            }
            int id = GetGameID();
            CreateGame(id);
            return id;
        }

        private int GetGameID()
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select max(id) from game_info";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.Read())
                            return ResultSet.GetInt32(0);
                }
            }
            return 0;
        }

        private void CreateGame(int id)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = $"create table game_{id} (move int, player int)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.ExecuteNonQuery();
                }
                for(int move = 1; move <= 9; move++)
                {
                    Query = $"insert into game_{id} (move, player) values (@Move, @Player)";
                    using (Statement = new SqlCommand(Query, Connection))
                    {
                        Statement.CommandType = CommandType.Text;
                        Statement.Parameters.AddWithValue("@Move", move);
                        Statement.Parameters.AddWithValue("@Player", 0);
                        Statement.ExecuteNonQuery();
                    }
                }
            }
        }

        public bool IsGameID(int gameID)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select * from game_info where id = (@GameID)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.HasRows)
                            return true;
                    return false;
                }
            }
        }

        public bool CheckPlayerInGameByID(string apiKey, int gameID)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select * from game_info where id = (@GameID) and (player_one_api_key = (@APIKey) or player_two_api_key = (@APIKey))";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    Statement.Parameters.AddWithValue("@APIKey", apiKey);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.HasRows)
                            return true;
                    return false;
                }
            }
        }

        public bool ChechPlayerMoveInGameByID(string apiKey, int gameID)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select player_one_api_key, player_two_api_key, moves from game_info where id = (@GameID)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.Read())
                            return CheckPlayerMove(apiKey, ResultSet.GetString(0), ResultSet.GetString(1), ResultSet.GetInt32(2));
                    return false;
                }
            }
        }

        private bool CheckPlayerMove(string apiKey, string playerOneAPIKey, string playerTwoAPIKey, int moves)
        {
            return (moves % 2 == 0) ? playerOneAPIKey.Equals(apiKey) : playerTwoAPIKey.Equals(apiKey);
        }

        public bool CheckMoveIsAvailableInGame(int gameID, int move)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = $"select player from game_{gameID} where move = (@Move)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@Move", move);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.Read())
                            return CheckMoveIsAvailable(ResultSet.GetInt32(0));
                }
            }
            return false;
        }

        private bool CheckMoveIsAvailable(int player)
        {
            return player == 0;
        }

        public void MakeAMove(int gameID, int move, string apiKey)
        {
            int player = GetPlayer(apiKey, gameID);
            int moves = GetMovesinGame(gameID) + 1;
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = $"update game_{gameID} set player = (@Player) where move = (@Move)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@Player", player);
                    Statement.Parameters.AddWithValue("@Move", move);
                    Statement.ExecuteNonQuery();
                }
                Query = "update game_info set moves = (@Moves) where id = (@GameID)";
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID); 
                    Statement.Parameters.AddWithValue("@Moves", moves);
                    Statement.ExecuteNonQuery();
                }
            }
        }

        private int GetPlayer(string apiKey, int gameID)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select player_one_api_key, player_two_api_key from game_info where id = (@GameID)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.Read())
                            return GetPlayer(apiKey, ResultSet.GetString(0), ResultSet.GetString(1));
                    return 0;
                }
            }
        }

        private int GetPlayer(string apiKey, string playerOneAPIKey, string playerTwoAPIKey)
        {
            return playerOneAPIKey.Equals(apiKey) ? 1 : playerTwoAPIKey.Equals(apiKey) ? 2 : 0;
        }

        private int GetMovesinGame(int gameID)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select moves from game_info where id = (@GameID)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.Read())
                            return ResultSet.GetInt32(0);
                    return 0;
                }
            }
        }

        public string GetStatus(int gameID)
        {
            int[,] game = new int[3,3];
            int moves = 0;
            string status = "";
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = $"select player from game_{gameID}";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        for (int row = 0; row < 3; row++)
                            for (int column = 0; column < 3; column++)
                                if (ResultSet.Read())
                                {
                                    game[row, column] = ResultSet.GetInt32(0);
                                    moves = game[row, column] == 0 ? moves : moves + 1;
                                }
                }
                status = CheckGameBoard(game, moves);
                Query = "update game_info set status = (@Status) where id = (@GameID)";
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    Statement.Parameters.AddWithValue("@Status", status);
                    Statement.ExecuteNonQuery();
                }
            }
            return status;
        }

        private string CheckGameBoard(int[,] game, int moves)
        {
            bool check;
            check = (
                (game[0, 0] == 1 && game[0, 1] == 1 && game[0, 2] == 1) || 
                (game[1, 0] == 1 && game[1, 1] == 1 && game[1, 2] == 1) || 
                (game[2, 0] == 1 && game[2, 1] == 1 && game[2, 2] == 1) ||
                (game[0, 0] == 1 && game[1, 0] == 1 && game[2, 0] == 1) ||
                (game[0, 1] == 1 && game[1, 1] == 1 && game[2, 1] == 1) ||
                (game[0, 2] == 1 && game[1, 2] == 1 && game[2, 2] == 1) ||
                (game[0, 0] == 1 && game[1, 1] == 1 && game[2, 2] == 1) ||
                (game[0, 2] == 1 && game[1, 1] == 1 && game[2, 0] == 1)
                );
            if (check)
                return "Player 1 Won";
            check = (
                (game[0, 0] == 2 && game[0, 1] == 2 && game[0, 2] == 2) ||
                (game[1, 0] == 2 && game[1, 1] == 2 && game[1, 2] == 2) ||
                (game[2, 0] == 2 && game[2, 1] == 2 && game[2, 2] == 2) ||
                (game[0, 0] == 2 && game[1, 0] == 2 && game[2, 0] == 2) ||
                (game[0, 1] == 2 && game[1, 1] == 2 && game[2, 1] == 2) ||
                (game[0, 2] == 2 && game[1, 2] == 2 && game[2, 2] == 2) ||
                (game[0, 0] == 2 && game[1, 1] == 2 && game[2, 2] == 2) ||
                (game[0, 2] == 2 && game[1, 1] == 2 && game[2, 0] == 2)
                );
            if (check)
                return "Player 2 Won";
            if (moves == 9)
                return "Tie";
            return "In Progress";
        }

        public bool CheckStatus(int gameID)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "select status from game_info where id = (@GameID)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@GameID", gameID);
                    using (SqlDataReader ResultSet = Statement.ExecuteReader())
                        if (ResultSet.Read())
                            return ResultSet.GetString(0) == ("In Progess");
                    throw new Exception();
                }
            }
        }
    }
}