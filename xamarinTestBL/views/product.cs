﻿using System;
using SQLite;

namespace xamarinTestBL
{
    public partial class views
    {
        public class product
        {
            [PrimaryKey, NotNull] public Guid id { get; set; }
            [NotNull] public string productCode { get; set; }
            [NotNull] public string productName { get; set; }
            [NotNull] public decimal currentPrice { get; set; }
            public Guid categoryUID { get; set; }
            public string productImage { get; set; }
            [NotNull] public int updateType { get; set; }
            public DateTime createdDate { get; set; }
            public DateTime editedDate { get; set; }
        }
    }
}