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

        internal void Get(string tableToUpdate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    if (tableToUpdate.Equals("coding"))
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
                    } else if (tableToUpdate.Equals("goals"))
                    {
                        List<GoalBlock> tableData = new List<GoalBlock>();
                        tableCmd.CommandText = "SELECT * FROM goals";

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
                                        Type = reader.GetString(4)
                                    });
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

        internal void Post(object obj, string tableToUpdate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    if (tableToUpdate.Equals("coding"))
                    {
                        // Typecasting to type CodeBlock
                        CodeBlock codeBlock = (CodeBlock) obj;
                        tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ('{codeBlock.Date}', '{codeBlock.Duration}')";
                    } else if (tableToUpdate.Equals("goals")) {
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

        internal string Filter(List<String> filters, List<String> columns, string table)
        {
            // String builder method. Returns full commandText string for tableCmd in Get()
            string returnString = $"SELECT * FROM {table}";

            if (columns.Count == 0)
            {
                return returnString;
            } else
            {
                returnString += " WHERE";
            }

            for (int i = 0; i < columns.Count; i++)
            {
                returnString += $" {columns[i]} = {filters[i]}";
                if (i >= 1)
                {
                    returnString += ",";
                }
            }
        }


    }
}
