using CodingTrackerV2.Models;
using ConsoleTableExt;
using System;
using System.Collections.Generic;

namespace CodingTrackerV2.Data
{
    internal class TableVisualization
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.WriteLine("\n");

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Coding")
                .ExportAndWriteLine();

            Console.WriteLine("\n");
        }
    }
}