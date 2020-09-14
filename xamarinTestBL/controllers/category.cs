using System.Text;

namespace xamarinTestBL
{
    public partial class controllers
    {
        public class category
        {
            public static void exportListCategory()
            {
                var listCategory = views.category.getListCategoryForExport();
                var sb = new StringBuilder();

                sb.AppendLine("\"Category Code\", \"Category Name\"");
                foreach (var category in listCategory)
                    sb.AppendLine("\"" + category.categoryCode + "\", \"" + category.categoryName + "\"");

                system.sysTool.writeToFile("category.csv", sb.ToString());
                system.sysTool.shareFile("category.csv");
            }
        }
    }
}
