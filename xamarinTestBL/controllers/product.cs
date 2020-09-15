using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace xamarinTestBL
{
    public partial class controllers
    {
        public class product
        {
            public static void exportListProduct()
            {
                var listCategory = views.category.getListCategoryForExport();
                var listProduct = views.product.getListProducts();
                var sb = new StringBuilder();

                sb.AppendLine("Category");
                sb.AppendLine("Code,Name,Updated");
                foreach (var category in listCategory)
                    sb.AppendLine(category.categoryCode + "," + category.categoryName);

                sb.AppendLine("");
                sb.AppendLine("Product");
                sb.AppendLine("Code,Name,Brand Variation,Qty Pack,Qty Piece,Price,Store,Category,Updated");
                foreach (var prod in listProduct)
                {
                    var categoryCode = listCategory.Where(cat => cat.id == prod.categoryUID).Select(cat => cat.categoryCode).FirstOrDefault();
                    sb.AppendLine(prod.productCode + "," + prod.productName + "," + prod.productBrand + "," + prod.productVariation + "," +
                                  prod.productPack_Initial + "," + prod.productPiece_Initial + "," + prod.productPrice_Initial + "," +
                                  prod.productStore + "," + categoryCode);
                }

                system.sysTool.writeToFile("rspa_export.csv", sb.ToString());
                system.sysTool.shareFile("Export RSPA Data", "rspa_export.csv");
            }

            public static void importListProduct(string filepath)
            {
                var listCategory = new List<views.category>();
                var listProduct = new List<views.product>();

                using (var reader = new StreamReader(filepath))
                {
                    var skipTwice = false; // skip title and header
                    var skip = 0;
                    string type = "";

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (values[0] == "Category" || values[0] == "Product")
                        {
                            type = values[0];
                            skipTwice = true;
                            skip++;
                            continue;
                        }

                        if (skipTwice && skip == 1)
                        {
                            skip++;
                            continue;
                        }

                        if (skipTwice && skip == 2)
                        {
                            skipTwice = false;
                            skip = 0;
                        }

                        if (string.IsNullOrEmpty(values[0]))
                            continue;

                        if (type == "Category")
                        {
                            if (values[2] == "1")
                            {
                                var nrec = new views.category();
                                nrec.categoryCode = values[0];
                                nrec.categoryName = values[1];
                                listCategory.Add(nrec);
                            }
                        }
                        else if (type == "Product")
                        {
                            if (values[9] == "1")
                            {
                                var nrec = new views.product();
                                // assign values
                                listProduct.Add(nrec);
                            }
                        }
                    }
                }

                // Update Categories
                var listExistingCategories = views.category.getListCategory();
                foreach (var category in listCategory)
                {
                    var exists = listExistingCategories.Where(cat => cat.categoryCode == category.categoryCode).FirstOrDefault();
                    if (exists != null)
                        exists.categoryName = category.categoryName;
                    else
                    {
                        // create new category
                    }
                }

                // Update Products
            }
        }
    }
}
