using System;
using System.Collections.Generic;
using System.Text;

namespace xamarinTestBL.services
{
    public interface IGetFile
    {
        string GetFile(string filename);
        string GetDirectory();
    }
}
