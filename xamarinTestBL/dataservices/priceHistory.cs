using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace xamarinTestBL
{
    public partial class dataservices
    {
        public class priceHistory
        {
            public static void addPriceHistory(views.priceHistory priceHistory)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Insert(priceHistory);
                }
            }

            public static void addListPriceHistory(List<views.priceHistory> listPriceHistory)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.InsertAll(listPriceHistory);
                }
            }

            public static List<views.priceHistory> GetListPriceHistoryForListview(Guid productUID)
            {
                var listPriceHistory = new List<views.priceHistory>();

                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT loggedDate, store, price FROM priceHistory WHERE productUID='" + productUID.ToString() + "' ORDER BY loggedDate DESC;";
                    listPriceHistory = conn.Query<views.priceHistory>(sql).ToList();
                }

                return listPriceHistory;
            }
        }
    }
}
