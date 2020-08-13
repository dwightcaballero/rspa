using System;
using SQLite;

namespace xamarinTestBL
{
    public partial class views
    {
        public class codeReference
        {
            [PrimaryKey, NotNull] public Guid id { get; set; }
            [NotNull] public string month { get; set; }
            [NotNull] public string year { get; set; }
            [NotNull] public int code { get; set; }
            [Ignore] public string productCode { get; set; }

            public static codeReference getCodeReference()
            {
                return dataservices.codeReference.getCodeReference();
            }
        }
    }
}
