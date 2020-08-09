using System;
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
        }
    }
}
