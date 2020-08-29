using SQLite;

namespace xamarinTestBL
{
    public partial class dataservices
    {
        public class product
        {
            public static void saveProduct(views.product product)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Insert(product);
                }
            }
        }
    }
}
