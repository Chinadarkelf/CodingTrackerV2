using CodingTrackerV2.Data;
using CodingTrackerV2.Models;
using System;
using System.Globalization;

namespace CodingTrackerV2
{
    internal class InfoController
    {
        // Handle codeBlock objects with Add, Update, Read, Delete methods -- auto/manual overloads

        bool closeApp = false;
        internal void ShowMenu()
        {
            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine(
                @"|======== Coding Tracker ========|
|                                |
|======= Select an option =======|
|==                            ==|
|==   1. Close Application     ==|
|==   2. View a Record         ==|
|==   3. Add a Record          ==|
|==   4. Delete a Record       ==|
|==   5. Update a Record       ==|
|==                            ==|
|================================|");

                Console.WriteLine("\n");
                string command = GetUserInput("Menu Option");

                switch (command)
                {
                    case "1":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "2":
                        RecordView();
                        Console.WriteLine("Press any key to return to main menu");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "3":
                        RecordAdd();
                        break;
                    case "4":
                        RecordDelete();
                        break;
                    case "5":
                        RecordUpdate(); // NOT IMPLEMENTED
                        break;
                    default:
                        Console.WriteLine("\nInvalid command. Please try again.");
                        break;
                }
            }
        }

        private string GetUserInput(string request)
        {
            var userInput = Console.ReadLine();

            switch (request)
            {
                case "Menu Option":
                    return userInput;
                    break;
                case "Get Date":
                    if (userInput.Equals(""))
                    {
                        // "d" returns short date string value (no time included)
                        return DateTime.Today.ToString("d");
                    }
                    while (!DateTime.TryParseExact(userInput, "dd-mm-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
                    {
                        Console.WriteLine("\nIncorrect format, try again");
                        userInput = Console.ReadLine();
                    }
                    return userInput;
                    break;
                case "Get Duration":
                    if (userInput.Equals("0")) ShowMenu();

                    while (!TimeSpan.TryParseExact(userInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
                    {
                        Console.WriteLine("\nDuration invalid, please try again. Format hh:mm, or type 0 to return to menu");
                        userInput= Console.ReadLine();

                        if (userInput.Equals("0")) ShowMenu();
                    }

                    return userInput;
                    break;
                case "Get Id":
                    while (!Int32.TryParse(userInput, out _) || string.IsNullOrEmpty(userInput) || Int32.Parse(userInput) < 0)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a valid integer");
                    }
                    return userInput;
                    break;
            }

            return "something went wrong...";
        }

        private void RecordUpdate()
        {
            CodeBlock codeBlock = new CodeBlock();
            RepoController repo = new RepoController();
            RecordView();

            Console.WriteLine("\nPlease enter the id of the record you wish to update");
            int idToUpdate = Int32.Parse(GetUserInput("Get Id"));

            Console.WriteLine("\nWhich column would you like to update? Type 'd' for date, 't' for duration, 's' for all, or 'q' to exit to main menu: ");
            string choice = Console.ReadLine();

            string dateInput = "";
            string durationInput = "";

            switch (choice)
            {
                case "d":
                    Console.WriteLine("\nPlease enter the date (format: dd-mm-yy). If nothing is entered, the current system time will be logged: ");
                    dateInput = GetUserInput("Get Date");

                    codeBlock.Date = dateInput;
                    repo.Update(idToUpdate, codeBlock, choice);
                    break;
                case "t":
                    Console.WriteLine("\nPlease enter the duration of the session (format hh:mm). Enter 0 to return to menu.");
                    durationInput = GetUserInput("Get Duration");

                    codeBlock.Duration = durationInput;
                    repo.Update(idToUpdate, codeBlock, choice);
                    break;
                case "s":
                    Console.WriteLine("\nPlease enter the date (format: dd-mm-yy). If nothing is entered, the current system time will be logged: ");
                    dateInput = GetUserInput("Get Date");

                    Console.WriteLine("\nPlease enter the duration of the session (format hh:mm). Enter 0 to return to menu.");
                    durationInput = GetUserInput("Get Duration");

                    codeBlock.Date = dateInput;
                    codeBlock.Duration = durationInput;
                    repo.Update(idToUpdate, codeBlock, choice);
                    break;
                case "q":
                    break;
            }

        }

        private void RecordDelete()
        {
            RepoController repo = new RepoController();
            RecordView(); // May need to change when filter functionality is added

            Console.WriteLine("\nPlease enter the id you wish to delete: ");
            int idToDelete = Int32.Parse(GetUserInput("Get Id"));

            repo.Delete(idToDelete);
        }

        private void RecordAdd()
        {
            RepoController repo = new RepoController();
            Console.WriteLine("\nPlease enter the date (format: dd-mm-yy). If nothing is entered, the current system time will be logged: ");
            string dateInput = GetUserInput("Get Date");

            // Console.WriteLine($"Added {dateInput}"); // DEBUG

            Console.WriteLine("\nPlease enter the duration of the session (format hh:mm). Enter 0 to return to menu.");
            string durationInput = GetUserInput("Get Duration");

            // Console.WriteLine($"\nAdded {durationInput}"); // DEBUG

            CodeBlock codeBlock = new CodeBlock();
            codeBlock.Date = dateInput;
            codeBlock.Duration = durationInput;

            repo.Post(codeBlock);
        }

        private void RecordView()
        {
            // REPO READ()
            RepoController repo = new RepoController();

            repo.Get();
        }
    }
}