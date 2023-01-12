using CodingTrackerV2.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CodingTrackerV2.Data
{
    internal class RepoController
    {
        // ConfigurationManager.AppSettings.Get("connectionString"); // Connection String for tracker.db
        // ConfigurationManager.AppSettings.Get("connectionStringGoals"); // Connection String for goals.db
        string connectionString = "";

        public RepoController(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal void Delete(int idToDelete, string tableToUpdate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    tableCmd.CommandText = $"DELETE FROM {tableToUpdate} WHERE Id = {idToDelete}";

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
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
                    {
                        List<CodeBlock> tableData = new List<CodeBlock>();
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
                            }
                            else
                            {
                                Console.WriteLine("\nNo rows found in table");
                            }
                        }

                        TableVisualization.ShowTable(tableData);
                    } else if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionStringGoals")))
                    {
                        List<GoalBlock> tableData = new List<GoalBlock>();
                        tableCmd.CommandText = "SELECT * FROM goals";

                        using (var reader = tableCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GoalBlock toAdd = new GoalBlock
                                    {
                                        Id = reader.GetInt32(0),
                                        Hours = reader.GetInt32(1),
                                        startDate = reader.GetString(2),
                                        endDate = reader.GetString(3),
                                        Type = reader.GetString(4),
                                        IsActive = DateTimeCalculator.Status(reader.GetString(2), reader.GetString(3)),
                                        Progress = reader.GetInt32(6)
                                    };
                                    tableData.Add(toAdd);
                                }
                            } else
                            {
                                Console.WriteLine("\nNo rows found in table");
                            }
                        }

                        TableVisualization.ShowTable(tableData);
                    }
                }
            }
        }

        // Overloaded method used for Update()
        internal object Get(int idToRetrieve)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    List<object> tableData = new List<object>();

                    if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
                    {                        
                        tableCmd.CommandText = $"SELECT * FROM coding WHERE Id = {idToRetrieve}";

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
                            }
                            else
                            {
                                Console.WriteLine("\nNo rows found in table");
                            }
                        }
                    } else if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionStringGoals")))
                    {
                        tableCmd.CommandText = $"SELECT * FROM goals WHERE Id = {idToRetrieve}";

                        using (var reader = tableCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    tableData.Add(new GoalBlock
                                    {
                                        Id = reader.GetInt32(0),
                                        Hours = reader.GetInt32(1),
                                        startDate = reader.GetString(2),
                                        endDate = reader.GetString(3),
                                        Type = reader.GetString(4),
                                        IsActive = reader.GetBoolean(5),
                                        Progress = reader.GetInt32(6)
                                    });
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNo rows found in table");
                            }
                        }
                    }
                    return tableData[0];
                }
            }
        }

        internal void Post(object obj)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
                    {
                        // Typecasting to type CodeBlock
                        CodeBlock codeBlock = (CodeBlock) obj;
                        tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ('{codeBlock.Date}', '{codeBlock.Duration}')";
                    } else if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionStringGoals"))) {
                        GoalBlock goalBlock = (GoalBlock) obj;
                        tableCmd.CommandText = $"INSERT INTO goals (hours, type, startdate, enddate, isactive, progress) VALUES ('{goalBlock.Hours}', '{goalBlock.Type}', '{goalBlock.startDate}', '{goalBlock.endDate}', '{goalBlock.IsActive}', '{goalBlock.Progress}')";
                    } else
                    {
                        Console.WriteLine("\nUnable to insert data into table, press any key to return to main menu");
                        Console.ReadLine();
                    }
                    
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void Update(int idToUpdate, object obj)
        {
            if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                CodeBlock codeBlock = (CodeBlock)obj;
                using (var connection = new SqliteConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();

                        tableCmd.CommandText =
                                    $@"UPDATE coding
                                       SET date = '{codeBlock.Date}',
                                           duration = '{codeBlock.Duration}'
                                       WHERE Id = {idToUpdate}";

                        int rowsAffected = tableCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Updated Record with Id {idToUpdate}.");
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Record with Id {idToUpdate} was not found.");
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadLine();
                        }
                    }
                }
            }
            else if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionStringGoals")))
            {
                GoalBlock goalBlock = (GoalBlock)obj;
                using (var connection = new SqliteConnection(connectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();

                        tableCmd.CommandText =
                                    $@"UPDATE goals
                                       SET hours = '{goalBlock.Hours}',
                                           startDate = '{goalBlock.startDate}',
                                           endDate = '{goalBlock.endDate}',
                                           type = '{goalBlock.Type}',
                                           isActive = '{goalBlock.IsActive}',
                                           progress = '{goalBlock.Progress}'
                                       WHERE Id = {idToUpdate}";

                        int rowsAffected = tableCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Updated Record with Id {idToUpdate}.");
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadLine();
                        }
                        else
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
}
