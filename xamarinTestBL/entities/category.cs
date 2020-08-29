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
        }
    }
}
