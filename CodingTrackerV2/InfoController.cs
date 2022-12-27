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
                        Console.WriteLine("Invalid command. Please try again.");
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
                    while (!DateTime.TryParseExact(userInput, "dd-mm-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
                    {
                        Console.WriteLine("Incorrect format, try again");
                        userInput = Console.ReadLine();
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
            Console.WriteLine("Please enter the date (format: dd-mm-yy). If nothing is entered, the current system time will be logged: ");

            string dateInput = GetUserInput("Get Date");
        }

        private void RecordView()
        {
            throw new NotImplementedException();
        }
    }
}