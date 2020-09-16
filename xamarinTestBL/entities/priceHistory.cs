using System.Collections.Generic;

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

            public static void addListPriceHistory(List<views.priceHistory> listPriceHistory)
            {
                dataservices.priceHistory.addListPriceHistory(listPriceHistory);
            }
        }
    }
}
