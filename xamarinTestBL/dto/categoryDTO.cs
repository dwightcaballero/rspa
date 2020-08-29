using System;
using System.Collections.Generic;
using System.Text;

namespace xamarinTestBL
{
    public partial class dto
    {
        public class categoryDTO
        {
            public views.category category { get; set; }
            public views.codeReference codeReference { get; set; }

            public categoryDTO()
            {
                category = new views.category();
                codeReference = new views.codeReference();
            }
        }
    }
}
