using CodingTrackerV2.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Configuration;

namespace CodingTrackerV2.Data
{
    internal class RepoController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        internal void Post(CodeBlock codeBlock)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ('{codeBlock.Date}', '{codeBlock.Duration}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
