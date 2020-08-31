using System;
using System.Collections.Generic;
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

            public static List<category> getListCategoryForListview()
            {
                return dataservices.category.getListCategoryForListview();
            }

            public static category getCategoryByID(Guid categoryUID)
            {
                return dataservices.category.getCategoryByID(categoryUID);
            }
        }
    }
}
