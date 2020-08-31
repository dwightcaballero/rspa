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
            [NotNull] public string productUID { get; set; }
            [NotNull] public decimal priceFrom { get; set; }
            [NotNull] public decimal priceTo { get; set; }
            [NotNull] public int updateType { get; set; }
            [NotNull] public DateTime loggedDate { get; set; }

            public static List<priceHistory> GetListPriceHistory(Guid productUID)
            {
                return new List<priceHistory>();
            }
        }
    }
}
