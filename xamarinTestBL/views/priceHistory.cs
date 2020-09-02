using System;
using System.Collections.Generic;
using SQLite;

namespace xamarinTestBL
{
    public partial class views
    {
        public class priceHistory
        {
            [PrimaryKey, NotNull] public Guid id { get; set; }
            [NotNull] public Guid productUID { get; set; }
            [NotNull] public decimal price { get; set; }
            [NotNull] public string store { get; set; }
            [NotNull] public int updateType { get; set; }
            [NotNull] public DateTime loggedDate { get; set; }

            public static List<priceHistory> GetListPriceHistoryForListview(Guid productUID)
            {
                return dataservices.priceHistory.GetListPriceHistoryForListview(productUID);
            }
        }
    }
}
