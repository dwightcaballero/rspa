using System.Collections.Generic;

namespace xamarinTestBL
{
    public partial class entities
    {
        public class product
        {
            public static void saveProduct(views.product product)
            {
                dataservices.product.saveProduct(product);
            }

            public static void updateProduct(views.product product)
            {
                dataservices.product.updateProduct(product);
            }

            public static void deleteProduct(views.product product)
            {
                dataservices.product.deleteProduct(product);
            }

            public static void saveListProduct(List<views.product> lisProduct)
            {
                dataservices.product.saveListProduct(lisProduct);
            }

            public static void updateListProduct(List<views.product> lisProduct)
            {
                dataservices.product.updateListProduct(lisProduct);
            }
        }
    }
}
