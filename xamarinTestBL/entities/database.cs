namespace xamarinTestBL
{
    public partial class entities
    {
        public class database
        {
            public static void initializeDatabase()
            {
                dataservices.database.initializeDatabase();
            }

            public static void resetDatabase()
            {
                dataservices.database.resetDatabase();
            }
        }
    }
}
