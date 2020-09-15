using SQLite;
using System;
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

            public static void updateCategory(views.category category)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Update(category);
                }
            }

            public static void deleteCategory(views.category category)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Delete(category);
                }
            }

            public static List<views.category> getListCategoryForListview()
            {
                var listCategories = new List<views.category>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT id, categoryName, categoryImage FROM category ORDER BY categoryName ASC;";
                    listCategories = conn.Query<views.category>(sql).ToList();
                }

                return listCategories;
            }

            public static List<views.category> getListCategoryForExport()
            {
                var listCategories = new List<views.category>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT id, categoryName, categoryCode FROM category ORDER BY categoryName ASC;";
                    listCategories = conn.Query<views.category>(sql).ToList();
                }

                return listCategories;
            }

            public static List<views.category> getListCategory()
            {
                var listCategory = new List<views.category>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    listCategory = conn.Table<views.category>().ToList();
                }

                return listCategory;
            }

            public static views.category getCategoryByID(Guid categoryUID)
            {
                views.category category = null;
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT * FROM category WHERE id='" + categoryUID.ToString() + "';";
                    category = conn.Query<views.category>(sql).FirstOrDefault();
                }

                return category;
            }
        }
    }
}
