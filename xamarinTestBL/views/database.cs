namespace xamarinTestBL
{
    public partial class views
    {
        public class database
        {
            public static bool databaseExists()
            {
                return dataservices.database.databaseExists();
            }

            public static string getDatabasePath()
            {
                return dataservices.database.DatabasePath;
            }
        }
    }
}
