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
                        break;
                    case "3":
                        RecordAdd();
                        break;
                    case "4":
                        RecordDelete();
                        break;
                    case "5":
                        RecordUpdate();
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
            }

            return "something went wrong...";
        }

        private void RecordUpdate()
        {
            throw new NotImplementedException();
        }

        private void RecordDelete()
        {
            throw new NotImplementedException();
        }

        private void RecordAdd()
        {
            Console.WriteLine("\nPlease enter the date (format: dd-mm-yy). If nothing is entered, the current system time will be logged: ");
            string dateInput = GetUserInput("Get Date");

            // Console.WriteLine($"Added {dateInput}"); // DEBUG

            Console.WriteLine("\nPlease enter the duration of the session (format hh:mm). Enter 0 to return to menu.");
            string durationInput = GetUserInput("Get Duration");

            Console.WriteLine($"\nAdded {durationInput}");
        }

        private void RecordView()
        {
            throw new NotImplementedException();
        }
    }
}