using SQLite;
using System;
using System.IO;

namespace xamarinTestBL
{
    public partial class dataservices
    {
        public class database
        {
            public const string DatabaseFilename = "rspa_db.db3";

            public const SQLite.SQLiteOpenFlags Flags =
                // open the database in read/write mode
                SQLite.SQLiteOpenFlags.ReadWrite |
                // create the database if it doesn't exist
                SQLite.SQLiteOpenFlags.Create |
                // enable multi-threaded database access
                SQLite.SQLiteOpenFlags.SharedCache;

            public static string DatabasePath
            {
                get
                {
                    var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    return Path.Combine(basePath, DatabaseFilename);
                }
            }

            // check if database exists
            public static bool databaseExists()
            {
                return File.Exists(DatabasePath);
            }

            // initialize the database
            public static void initializeDatabase()
            {
                using (SQLiteConnection conn = new SQLiteConnection(DatabasePath))
                {
                    // create tables
                    conn.CreateTable<views.category>();
                    conn.CreateTable<views.codeReference>();
                    conn.CreateTable<views.priceHistory>();
                    conn.CreateTable<views.product>();
                }
            }

            public static void resetDatabase()
            {
                using (SQLiteConnection conn = new SQLiteConnection(DatabasePath))
                {
                    // delete tables
                    conn.DropTable<views.category>();
                    conn.DropTable<views.codeReference>();
                    conn.DropTable<views.priceHistory>();
                    conn.DropTable<views.product>();
                }

                initializeDatabase();
            }
        }
    }
}
