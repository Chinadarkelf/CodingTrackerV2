using Microsoft.Data.Sqlite;

namespace CodingTrackerV2
{
    internal class DBManager
    {
        // static InfoController control = new InfoController();

        internal void CreateTable(string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
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
        }


    }
}
