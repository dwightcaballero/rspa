using SQLite;
using System;
using System.Linq;

namespace xamarinTestBL
{
    public partial class dataservices
    {
        public class codeReference
        {
            public static views.codeReference getCodeReference(int type)
            {
                string month = DateTime.Now.ToString("MM");
                string year = DateTime.Now.ToString("yy");

                views.codeReference codeReference = null;
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT * FROM codeReference WHERE month='" + month + "' AND year='" + year + "' AND type=" + type + ";";
                    codeReference = conn.Query<views.codeReference>(sql).FirstOrDefault();
                }

                if (codeReference == null)
                {
                    codeReference = new views.codeReference();
                    codeReference.id = Guid.NewGuid();
                    codeReference.month = month;
                    codeReference.year = year;
                    codeReference.type = type;
                    codeReference.code = 0;
                    codeReference.code_string = month + year + "0000";
                }
                else
                {
                    codeReference.code += 1;
                    codeReference.code_string = month + year + type + codeReference.code.ToString().PadLeft(4, '0');
                }

                return codeReference;
            }

            public static void updateCodeReference(views.codeReference codeReference)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    if (codeReference.code == 0)
                        conn.Insert(codeReference);
                    else{
                        conn.Update(codeReference);
                    }
                }
            }
        }
    }
}
