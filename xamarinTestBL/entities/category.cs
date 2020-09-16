using System.Collections.Generic;

namespace xamarinTestBL
{
    public partial class entities
    {
        public class category
        {
            public static void saveCategory(views.category category)
            {
                dataservices.category.saveCategory(category);
            }

            public static void updateCategory(views.category category)
            {
                dataservices.category.updateCategory(category);
            }

            public static void deleteCategory(views.category category)
            {
                dataservices.category.deleteCategory(category);
            }

            public static void saveListCategory(List<views.category> listCategory)
            {
                dataservices.category.saveListCategory(listCategory);
            }

            public static void updateListCategory(List<views.category> listCategory)
            {
                dataservices.category.updateListCategory(listCategory);
            }
        }
    }
}
