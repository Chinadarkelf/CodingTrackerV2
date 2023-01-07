using CodingTrackerV2.Data;
using CodingTrackerV2.Models;
using System;
using System.Configuration;
using System.Globalization;

namespace CodingTrackerV2
{
    internal class InfoController
    {
        // Handle codeBlock objects with Add, Update, Read, Delete methods -- auto/manual overloads

        bool closeApp = false;
        string trackerString = ConfigurationManager.AppSettings.Get("connectionString");
        string goalsString = ConfigurationManager.AppSettings.Get("connectionStringGoals");
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
|==   6. Current Goals         ==|
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
                        RecordView(trackerString);
                        Console.WriteLine("Press any key to return to main menu");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "3":
                        RecordAdd(trackerString);
                        break;
                    case "4":
                        RecordDelete(trackerString);
                        break;
                    case "5":
                        RecordUpdate(trackerString);
                        break;
                    case "6":
                        ViewGoalsMenu(goalsString);
                        break;
                    default:
                        Console.WriteLine("\nInvalid command. Please try again.");
                        break;
                }
            }
        }

        internal void ViewGoalsMenu(string connectionString)
        {
            Console.Clear();
            Console.WriteLine(
                @"|======== Goal Tracker ========|
|                              |
|====== Select an option ======|
|==                          ==|
|==   1. Return to Menu      ==|
|==   2. View Goals          ==|
|==   3. Add a Goal          ==|
|==   4. Delete a Goal       ==|
|==   5. Update a Goal       ==|
|==                          ==|
|==============================|");

            Console.WriteLine("\n");
            Console.WriteLine("Please select an option");
            string command = GetUserInput("Menu Option");

            switch (command)
            {
                case "1":
                    ShowMenu();
                    break;
                case "2":
                    RecordView(goalsString);
                    ViewGoalsMenu(goalsString);
                    break;
                case "3":
                    RecordAdd(goalsString);
                    break;
                case "4":
                    RecordDelete(goalsString); // NOT IMPLEMENTED
                    break;
                case "5":
                    RecordUpdate(goalsString); // NOT IMPLEMENTED
                    break;
                default:
                    // Console.WriteLine($"{command}"); // DEBUG
                    Console.WriteLine("\nInvalid Command. Please try again");
                    Console.ReadLine();
                    ViewGoalsMenu(goalsString);
                    break;
            }
        }

        private string GetUserInput(string request)
        {
            var userInput = Console.ReadLine();

            switch (request)
            {
                case "Menu Option":
                    return userInput;
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
                case "Get Duration":
                    if (userInput.Equals("0") || userInput.Equals("Q")) ShowMenu();

                    while (!TimeSpan.TryParseExact(userInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
                    {
                        Console.WriteLine("\nDuration invalid, please try again. Format hh:mm, or type 0 to return to menu");
                        userInput= Console.ReadLine();

                        if (userInput.Equals("0")) ShowMenu();
                    }

                    return userInput;
                case "Get Id":
                    while (!Int32.TryParse(userInput, out _) || string.IsNullOrEmpty(userInput) || Int32.Parse(userInput) < 0)
                    {
                        Console.WriteLine("\nInvalid input. Please enter a valid integer");
                    }
                    return userInput;
                case "Get Type":
                    if (userInput.Equals("0")) ViewGoalsMenu(goalsString);

                    while (!userInput.Equals("daily") || userInput.Equals("weekly") || userInput.Equals("monthly") || userInput.Equals("yearly"))
                    {
                        // Console.WriteLine($"{userInput}"); // DEBUG
                        Console.WriteLine("Invalid command. Please enter a valid option");
                        userInput = Console.ReadLine();

                        if (userInput.Equals("0")) ViewGoalsMenu(goalsString);
                    }

                    return userInput;
                case "Get Goal Duration":
                    while (!Int32.TryParse(userInput, out _))
                    {
                        Console.WriteLine("Invalid duration. Please enter a whole number!");
                        userInput = Console.ReadLine();
                    }
                    return userInput;
            }

            return $"Developer! Did you enter the wrong request string? Error getting {request}";
        }

        private void RecordUpdate(string connectionString)
        {
            CodeBlock codeBlock = new CodeBlock();
            RepoController repo = new RepoController(connectionString);
            RecordView(connectionString);

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

        private void RecordDelete(string connectionString)
        {
            RepoController repo = new RepoController(connectionString);
            RecordView(connectionString);

            Console.WriteLine("\nPlease enter the id you wish to delete: ");
            int idToDelete = Int32.Parse(GetUserInput("Get Id"));

            repo.Delete(idToDelete, "coding");
        }

        private void RecordAdd(string connectionString)
        {
            bool trackerSwitch = true;
            if (!connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                trackerSwitch = false;
            }
            switch (trackerSwitch)
            {
                case true:
                    RepoController repo = new RepoController(connectionString);
                    Console.WriteLine("\nPlease enter the date (format: dd-mm-yy). If nothing is entered, the current system time will be logged: ");
                    string dateInput = GetUserInput("Get Date");

                    // Console.WriteLine($"Added {dateInput}"); // DEBUG

                    Console.WriteLine("\nPlease enter the duration of the session (format hh). Enter Q to return to menu.");
                    string durationInput = GetUserInput("Get Duration");

                    // Console.WriteLine($"\nAdded {durationInput}"); // DEBUG

                    CodeBlock codeBlock = new CodeBlock();
                    codeBlock.Date = dateInput;
                    codeBlock.Duration = durationInput;

                    repo.Post(codeBlock, "coding");
                    break;

                case false:
                    RepoController repoGoals = new RepoController(connectionString);
                    bool breakOut = false;

                    while (!breakOut)
                    {
                        Console.WriteLine("\nPlease enter the type of goal (format: daily, weekly, monthly, yearly). Enter 0 to return to menu: ");
                        string typeInput = GetUserInput("Get Type");

                        Console.WriteLine("\nPlease enter the start date of your goal. If nothing is entered, the current system time will be given: ");
                        string startDateInput = GetUserInput("Get Date");

                        // Parse data for start and end date
                        string endDateInput = DateTimeCalculator.CalculateEndDate(typeInput, startDateInput);

                        Console.WriteLine($"Current end date is set for {endDateInput}. Enter '1' if this is correct, or '2' if incorrect: ");
                        if (GetUserInput("Menu Option").Equals("1")) breakOut = true;

                        Console.WriteLine("\nPlease enter how many hours you would like to complete in the given timespan (format: hh:mm): ");
                        int hours = Int32.Parse(GetUserInput("Get Goal Duration"));

                        GoalBlock goalBlock = new GoalBlock();
                        goalBlock.startDate = startDateInput;
                        goalBlock.endDate = endDateInput;
                        goalBlock.Type = typeInput;
                        goalBlock.Hours = hours;

                        //DETERMINE PROGRESS -- Pull information from tracker.db to determine how many hours have been accounted for during goal period


                        repoGoals.Post(goalBlock, "goals");
                    }
                    
                    break;
            }
        }

        private void RecordView(string connectionString)
        {
            // REPO READ()
            RepoController repo = new RepoController(connectionString);

            if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                repo.Get("coding");
            } else if (connectionString.Equals(ConfigurationManager.AppSettings.Get("connectionStringGoals")))
            {
                repo.Get("goals");
            }

            Console.WriteLine(@"Want to filter/adjust the table? Type h followed by the columns you want to hide
Or type f followed by the column you want to filter. You will be prompted for
The bounds you want to filter to. Type 0 to exit to menu.");
            Console.ReadLine();
        }
    }
}

