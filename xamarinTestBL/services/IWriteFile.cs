using System;
using System.Collections.Generic;
using System.Text;

namespace xamarinTestBL.services
{
    public interface IWriteFile
    {
        void WriteFile(string filename, string content);
    }
}
