using System;
using System.Collections.Generic;
using System.Text;

namespace xamarinTestBL
{
    public partial class entities
    {
        public class codeReference
        {
            public static void updateCodeReference(views.codeReference codeReference)
            {
                dataservices.codeReference.updateCodeReference(codeReference);
            }
        }
    }
}
