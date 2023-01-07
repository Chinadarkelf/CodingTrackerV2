using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTrackerV2
{
    internal class DBManager
    {
        // static InfoController control = new InfoController();

        internal void CreateTable(string connectionStringRequest)
        {
            if (connectionStringRequest.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                using (var connection = new SqliteConnection(connectionStringRequest))
                {
                    using (var tableCommand = connection.CreateCommand())
                    {
                        connection.Open();

                        tableCommand.CommandText =
                            @"CREATE TABLE IF NOT EXISTS coding (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Duration TEXT
                        )";

                        tableCommand.ExecuteNonQuery();
                    }
                }
            } else if (connectionStringRequest.Equals(ConfigurationManager.AppSettings.Get("connectionStringGoals")))
            {
                using (var connection = new SqliteConnection(connectionStringRequest))
                {
                    using (var tableCommand = connection.CreateCommand())
                    {
                        connection.Open();

                        tableCommand.CommandText =
                            @"CREATE TABLE IF NOT EXISTS goals (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            hours INTEGER,
                            startDate TEXT,
                            endDate TEXT,
                            Type TEXT,
                            IsActive BOOL,
                            Progress INTEGER
                        )";

                        tableCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
