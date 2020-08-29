using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace xamarinTestBL
{
    public partial class dataservices
    {
        public class category
        {
            public static void saveCategory(views.category category)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Insert(category);
                }
            }

            public static List<views.category> getListCategory()
            {
                var listCategories = new List<views.category>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT * FROM category ORDER BY categoryName ASC;";
                    listCategories = conn.Query<views.category>(sql).ToList();
                }

                return listCategories;
            }

            public static void updateCategory(views.category category)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Update(category);
                }
            }
        }
    }
}
