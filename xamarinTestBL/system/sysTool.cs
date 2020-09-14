using System;
using Xamarin.Forms;
using xamarinTestBL.services;

namespace xamarinTestBL
{
    public partial class system
    {
        public class sysTool
        {
            public static string readException(Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null) msg += "/" + ex.InnerException.Message;
                return msg;
            }

            public static void flushLogToFile(string filetype, string content)
            {
                string filename = "log" + filetype + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + "_" + Environment.MachineName + ".txt";
                content = DateTime.Now.Date.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString() + ", " + content;

                var logger = DependencyService.Get<IWriteFile>();
                logger.WriteFile(filename, content);
            }

            public static void writeToFile(string filename, string content)
            {
                var logger = DependencyService.Get<IWriteFile>();
                logger.WriteFile(filename, content);
            }
        }
    }
}
