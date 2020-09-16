using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

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

            public static void updateProduct(views.product product)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Update(product);
                }
            }

            public static void deleteProduct(views.product product)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.Delete(product);
                }
            }

            public static views.product getProductByID(Guid productUID)
            {
                views.product product = null;
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT * FROM product WHERE id='" + productUID.ToString() + "';";
                    product = conn.Query<views.product>(sql).FirstOrDefault();
                }

                return product;
            }

            public static List<views.product> getListProductsForListView()
            {
                var listProducts = new List<views.product>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT id, productImage, productBrand, productName, productVariation, categoryUID FROM product ORDER BY productBrand, productName, productVariation ASC;";
                    listProducts = conn.Query<views.product>(sql).ToList();
                }

                return listProducts;
            }

            public static List<views.product> getListProducts()
            {
                var listProducts = new List<views.product>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    listProducts = conn.Table<views.product>().ToList();
                }

                return listProducts;
            }

            public static List<views.product> getListProductByListProductCode(List<string> listProductCode)
            {
                var listProduct = new List<views.product>();
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    string sql = "SELECT * FROM product WHERE " + system.sysTool.buildOR(listProductCode, "productCode") + ";";
                    listProduct = conn.Query<views.product>(sql).ToList();
                }

                return listProduct;
            }

            public static void saveListProduct(List<views.product> listProduct)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.InsertAll(listProduct);
                }
            }

            public static void updateListProduct(List<views.product> listProduct)
            {
                using (SQLiteConnection conn = new SQLiteConnection(database.DatabasePath))
                {
                    conn.UpdateAll(listProduct);
                }
            }
        }
    }
}
