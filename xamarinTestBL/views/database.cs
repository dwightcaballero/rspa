using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
