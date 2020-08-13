using SQLite;
using System;
using System.Linq;

namespace xamarinTestBL
{
    public partial class dataservices
    {
        public class codeReference
        {
            public static views.codeReference getCodeReference()
            {
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yy");

                var codeReference = new views.codeReference();
                codeReference.month = month;
                codeReference.year = year;

                int? code;
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT code FROM codeReference ORDER BY code DESC LIMIT 1;";
                    code = conn.Query<int>(sql).FirstOrDefault();
                }

                if (code == null)
                {
                    codeReference.productCode = month + year + "0000";
                    codeReference.code = 0;
                }
                else
                {
                    codeReference.productCode = month + year + code.ToString().PadLeft(4, '0');
                    codeReference.code = code.Value;
                }

                return codeReference;
            }
        }
    }
}
