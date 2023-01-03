using CodingTrackerV2.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CodingTrackerV2.Data
{
    internal class RepoController
    {
        string connectionString = "";

        public RepoController(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal void Delete(int idToDelete)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    tableCmd.CommandText = $"DELETE FROM coding WHERE Id = {idToDelete}";

                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Succesfully deleted Record with Id {idToDelete}");
                        Console.WriteLine("\nPress any key to return to main menu");
                        Console.ReadLine();
                    } else
                    {
                        Console.WriteLine($"Record with ID {idToDelete} was not found");
                        Console.WriteLine("\nPress any key to return to main menu");
                        Console.ReadLine();
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

        internal void Update(int idToUpdate, CodeBlock codeBlock, string request)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    switch (request)
                    {
                        case "d":
                            tableCmd.CommandText =
                                $@"UPDATE coding
                                   SET date = '{codeBlock.Date}'
                                   WHERE Id = {idToUpdate}";
                            break;
                        case "t":
                            tableCmd.CommandText =
                                $@"UPDATE coding
                                   SET duration = '{codeBlock.Duration}'
                                   WHERE Id = {idToUpdate}";
                            break;
                        case "s":
                            tableCmd.CommandText =
                                $@"UPDATE coding
                                SET date = '{codeBlock.Date}',
                                    duration = '{codeBlock.Duration}'
                                WHERE Id = {idToUpdate}";
                            break;
                    }
                    int rowsAffected = tableCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Updated Record with Id {idToUpdate}.");
                        Console.WriteLine("Press any key to return to menu");
                        Console.ReadLine();
                    } else
                    {
                        Console.WriteLine($"Record with Id {idToUpdate} was not found.");
                        Console.WriteLine("Press any key to return to menu");
                        Console.ReadLine();
                    }
                }
            }
        }


    }
}
