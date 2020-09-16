using System;
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
                            if (!string.IsNullOrWhiteSpace(values[2]))
                            {
                                var newCategory = new views.category();
                                newCategory.categoryCode = values[0].PadLeft(9, '0');
                                newCategory.categoryCode_import = values[0].PadLeft(9, '0');
                                newCategory.categoryName = values[1].ToUpper().Trim();
                                newCategory.id = Guid.NewGuid();
                                newCategory.createdDate = DateTime.Now;
                                newCategory.editedDate = DateTime.Now;
                                newCategory.updateType = (int)system.sysConst.updateType.Import;
                                listCategory.Add(newCategory);
                            }
                        }
                        else if (type == "Product")
                        {
                            if (!string.IsNullOrWhiteSpace(values[9]))
                            {
                                var newProduct = new views.product();
                                newProduct.id = Guid.NewGuid();
                                newProduct.productCode = values[0].PadLeft(9, '0');
                                newProduct.productName = values[1].ToUpper().Trim();
                                newProduct.productBrand = values[2].ToUpper().Trim();
                                newProduct.productVariation = values[3].ToUpper().Trim();
                                newProduct.productPack_Initial = decimal.Parse(values[4].Trim());
                                newProduct.productPiece_Initial = decimal.Parse(values[5].Trim());
                                newProduct.productPrice_Initial = decimal.Parse(values[6].Trim());
                                newProduct.productStore = values[7].ToUpper().Trim();
                                newProduct.categoryCode = values[8];
                                newProduct.createdDate = DateTime.Now;
                                newProduct.editedDate = DateTime.Now;
                                newProduct.updateType = (int)system.sysConst.updateType.Import;

                                listProduct.Add(newProduct);
                            }
                        }
                    }
                }

                var listCategoryCode = listCategory.Select(cat => cat.categoryCode).ToList();
                var listExistingCategories = views.category.getListCategoryByListCategoryCode(listCategoryCode);
                var cat_codeReference = views.codeReference.getCodeReference((int)system.sysConst.codeReferenceType.Category);
                var listNewCategories = new List<views.category>();

                foreach (var category in listCategory)
                {
                    var exists = listExistingCategories.Where(cat => cat.categoryCode == category.categoryCode).FirstOrDefault();
                    if (exists != null)
                    {
                        category.id = exists.id;
                        exists.editedDate = DateTime.Now;
                        exists.updateType = (int)system.sysConst.updateType.Import;
                        exists.categoryName = category.categoryName;
                    }
                    else
                    {
                        category.categoryCode = cat_codeReference.code_string;
                        listNewCategories.Add(category);

                        cat_codeReference.code += 1;
                        cat_codeReference.code_string = cat_codeReference.month + cat_codeReference.year + cat_codeReference.type +
                                                        cat_codeReference.code.ToString().PadLeft(4, '0');
                    }
                }

                // Update Categories
                if (listExistingCategories.Count > 0) entities.category.updateListCategory(listExistingCategories);

                // Add Categories
                if (listNewCategories.Count > 0)
                {
                    entities.category.saveListCategory(listNewCategories);

                    // Update Code Reference
                    cat_codeReference.code -= 1;
                    entities.codeReference.updateCodeReference(cat_codeReference);
                }

                var listProductCode = listProduct.Select(prod => prod.productCode).ToList();
                var listExistingProducts = views.product.getListProductByListProductCode(listProductCode);
                var prod_codeReference = views.codeReference.getCodeReference((int)system.sysConst.codeReferenceType.Product);
                var listNewProducts = new List<views.product>();
                var listPricehistory = new List<views.priceHistory>();

                foreach (var product in listProduct)
                {
                    product.categoryUID = listCategory.Where(cat => cat.categoryCode_import == product.categoryCode).Select(cat => cat.id).FirstOrDefault();
                    product.productPrice = product.productPrice_Initial /= (product.productPack_Initial * product.productPiece_Initial);

                    var priceHistory = new views.priceHistory();
                    priceHistory.id = Guid.NewGuid();
                    priceHistory.price = product.productPrice;
                    priceHistory.store = product.productStore;
                    priceHistory.updateType = (int)system.sysConst.updateType.Import;
                    priceHistory.loggedDate = DateTime.Now;

                    var exists = listExistingProducts.Where(prod => prod.productCode == product.productCode).FirstOrDefault();
                    if (exists != null)
                    {
                        priceHistory.productUID = exists.id;

                        exists.productName = product.productName;
                        exists.productBrand = product.productBrand;
                        exists.productVariation = product.productVariation;
                        exists.productPack_Initial = product.productPack_Initial;
                        exists.productPiece_Initial = product.productPiece_Initial;
                        exists.productPrice_Initial = product.productPrice_Initial;
                        exists.productStore = product.productStore;
                        exists.categoryUID = product.categoryUID;
                        exists.editedDate = DateTime.Now;
                        exists.updateType = (int)system.sysConst.updateType.Import;
                    }
                    else
                    {
                        priceHistory.productUID = product.id;

                        product.productCode = prod_codeReference.code_string;
                        listNewProducts.Add(product);

                        prod_codeReference.code += 1;
                        prod_codeReference.code_string = prod_codeReference.month + prod_codeReference.year + prod_codeReference.type +
                                                         prod_codeReference.code.ToString().PadLeft(4, '0');
                    }

                    listPricehistory.Add(priceHistory);
                }

                // Update Products
                if (listExistingProducts.Count > 0) entities.product.updateListProduct(listExistingProducts);

                // Add Products
                if (listNewProducts.Count > 0)
                {
                    entities.product.saveListProduct(listNewProducts);

                    // Update Code Reference
                    prod_codeReference.code -= 1;
                    entities.codeReference.updateCodeReference(prod_codeReference);
                }

                // Add Price History
                if (listPricehistory.Count > 0) entities.priceHistory.addListPriceHistory(listPricehistory);
            }
        }
    }
}
