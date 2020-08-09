using System;
using SQLite;

namespace xamarinTestBL
{
    public partial class views
    {
        public class category
        {
            [PrimaryKey, NotNull] public Guid id { get; set; }
            [NotNull] public string categoryCode { get; set; }
            [NotNull] public string categoryName { get; set; }
            public string categoryImage { get; set; }
            [NotNull] public int updateType { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime editedDate { get; set; }
        }
    }
}
