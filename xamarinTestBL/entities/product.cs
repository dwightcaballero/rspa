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
        }
    }
}
