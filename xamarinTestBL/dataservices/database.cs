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
        }
    }
}
