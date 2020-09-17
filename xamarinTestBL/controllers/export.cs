using System.Linq;
using System.Text;

namespace xamarinTestBL
{
    public partial class controllers
    {
        public class export
        {
            public static void exportListProduct()
            {
                var listCategory = views.category.getListCategoryForExport();
                var listProduct = views.product.getListProducts();
                var sb = new StringBuilder();

                sb.AppendLine("Category");
                sb.AppendLine("Code,Name,Updated");
                foreach (var category in listCategory)
                    sb.AppendLine(category.categoryCode + "," + category.categoryName + ",");

                sb.AppendLine("");
                sb.AppendLine("Product");
                sb.AppendLine("Code,Name,Brand,Variation,Qty Pack,Qty Piece,Price,Store,Category,Updated");
                foreach (var prod in listProduct)
                {
                    var categoryCode = listCategory.Where(cat => cat.id == prod.categoryUID).Select(cat => cat.categoryCode).FirstOrDefault();
                    sb.AppendLine(prod.productCode + "," + prod.productName + "," + prod.productBrand + "," + prod.productVariation + "," +
                                  prod.productPack_Initial + "," + prod.productPiece_Initial + "," + prod.productPrice_Initial + "," +
                                  prod.productStore + "," + categoryCode + ",");
                }

                system.sysTool.writeToFile("export_product.csv", sb.ToString());
                system.sysTool.shareFile("Export RSPA Data", "rspa_export.csv");
            }
        }
    }
}
