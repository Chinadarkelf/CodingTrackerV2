using System.Configuration;

namespace CodingTrackerV2
{
    internal class Program
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        static string connectionStringGoals = ConfigurationManager.AppSettings.Get("connectionStringGoals");

        static void Main(string[] args)
        {
            DBManager dbManager = new DBManager();
            InfoController infoController = new InfoController();

            dbManager.CreateTable(connectionString);
            dbManager.CreateTable(connectionStringGoals);

            infoController.ShowMenu();
        }
    }
}
