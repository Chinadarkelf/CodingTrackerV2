using System.Configuration;

namespace CodingTrackerV2
{
    internal class Program
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        static void Main(string[] args)
        {
            DBManager dbManager = new DBManager();
            InfoController infoController = new InfoController();

            dbManager.CreateTable(connectionString);

            infoController.ShowMenu();
        }
    }
}
