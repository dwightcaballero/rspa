namespace xamarinTestBL
{
    public partial class entities
    {
        public class priceHistory
        {
            public static void addPriceHistory(views.priceHistory priceHistory)
            {
                dataservices.priceHistory.addPriceHistory(priceHistory);
            }
        }
    }
}
