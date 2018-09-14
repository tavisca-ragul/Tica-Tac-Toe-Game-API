using System;
using System.Data;
using System.Data.SqlClient;
using TicTacToe.Model;

namespace TicTacToe.Logging
{
    public class Logging : ILogging
    {
        SqlConnectionStringBuilder Builder;
        SqlConnection Connection;
        SqlCommand Statement;
        String Query;
        private static Logging _Instance = null;
        public static Logging Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Logging();
                return _Instance;
            }
        }

        private Logging()
        {
            Builder = new SqlConnectionStringBuilder();
            Builder.DataSource = "TAVDESK136";
            Builder.UserID = "sa";
            Builder.Password = "test123!@#";
            Builder.InitialCatalog = "TicTacToe";
            Query = null;
            Statement = null;
        }

        public void ProcessLogMessage(LogInfo logInfo)
        {
            using (Connection = new SqlConnection(Builder.ConnectionString))
            {
                Query = "insert into log_details(request, response, exception, comment) values(@Request, @Response, @Exception, @Comment)";
                Connection.Open();
                using (Statement = new SqlCommand(Query, Connection))
                {
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@Request", logInfo.Request);
                    Statement.Parameters.AddWithValue("@Response", logInfo.Response);
                    Statement.Parameters.AddWithValue("@Exception", logInfo.Exception);
                    Statement.Parameters.AddWithValue("@Comment", logInfo.Comment);
                    Statement.ExecuteNonQuery();
                }
            }
        }
    }
}