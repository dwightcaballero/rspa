using System.Collections.Generic;

namespace xamarinTestBL
{
    public partial class dto
    {
        public class productDTO
        {
            public views.product product { get; set; }
            public views.codeReference codeReference { get; set; }
            public List<views.category> listCategories { get; set; }

            public productDTO()
            {
                product = new views.product();
                codeReference = new views.codeReference();
                listCategories = new List<views.category>();
            }
        }
    }
}
