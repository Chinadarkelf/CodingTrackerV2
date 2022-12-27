using CodingTrackerV2.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CodingTrackerV2.Data
{
    internal class RepoController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        internal void Delete(int idToDelete)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    tableCmd.CommandText = $"DELETE FROM coding (id) WHERE id = {idToDelete}";

                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine($"Record with ID {idToDelete} was not found");
                    } else
                    {
                        Console.WriteLine($"Record with ID {idToDelete} was deleted");
                    }
                }
            }        
        }

        internal void Get()
        {
            List<CodeBlock> tableData = new List<CodeBlock>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";
                    
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(new CodeBlock
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetString(1),
                                    Duration = reader.GetString(2)
                                });
                            }
                        } else
                        {
                            Console.WriteLine("\nNo rows found in table");
                        }
                    }
                }
            }

            TableVisualization.ShowTable(tableData);
        }

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
