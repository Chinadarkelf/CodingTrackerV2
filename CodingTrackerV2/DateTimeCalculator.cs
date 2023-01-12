using System;

namespace CodingTrackerV2
{
    internal class DateTimeCalculator
    {
        internal static string CalculateEndDate(string typeInput, string startDateInput)
        {
            DateTime dt = DateTime.Parse(startDateInput);
            switch (typeInput)
            {
                case "daily":
                    DateTime dtDaily = dt.AddDays(1);
                    return dtDaily.ToString("d");
                case "weekly":
                    DateTime dtWeekly = dt.AddDays(7);
                    return dtWeekly.ToString("d");
                case "monthly":
                    DateTime dtMonthly = dt.AddDays(30);
                    return dtMonthly.ToString("d");
                case "yearly":
                    DateTime dtYearly = dt.AddDays(365);
                    return dtYearly.ToString("d");
                default:
                    return "Something went wrong! Did an invalid startDateInput / typeInput get passed?";
            }
        }

        internal static bool Status(string startDateInput, string endDateInput)
        {
            DateTime dtStart = DateTime.Parse(startDateInput);
            DateTime dtEnd = DateTime.Parse(endDateInput);

            if (DateTime.Now >= dtStart && DateTime.Now < dtEnd)
            {
                return true;
            } else
            {
                return false;
            }
        }

        internal static bool StatusOfTracker(string toPullDate, string goalStart, string goalEnd)
        {
            DateTime pullDate = DateTime.Parse(toPullDate);
            DateTime dtStart = DateTime.Parse(goalStart);
            DateTime dtEnd = DateTime.Parse(goalEnd);

            if (pullDate >= dtStart && pullDate < dtEnd)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
