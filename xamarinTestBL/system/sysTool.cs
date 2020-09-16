using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using xamarinTestBL.services;

namespace xamarinTestBL
{
    public partial class system
    {
        public class sysTool
        {
            static string dateInfile = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + "_";

            public static string buildOR(List<Guid> listIDs, string fieldName)
            {
                if (listIDs.Count == 0)
                {
                    return " 1=0";
                }

                StringBuilder sql = new StringBuilder();
                foreach (Guid ID in listIDs)
                {
                    if (!string.IsNullOrEmpty(sql.ToString()))
                    {
                        sql.Append(" or ");
                    }

                    sql.Append(fieldName);
                    sql.Append("='");
                    sql.Append(ID.ToString());
                    sql.Append("' ");
                }

                return sql.ToString();
            }

            public static string buildOR(List<string> listString, string fieldName)
            {
                if (listString.Count == 0)
                {
                    return " 1=0";
                }

                StringBuilder sql = new StringBuilder();
                foreach (string ID in listString)
                {
                    if (!string.IsNullOrEmpty(sql.ToString()))
                    {
                        sql.Append(" or ");
                    }

                    sql.Append(fieldName);
                    sql.Append("='");
                    sql.Append(ID.ToString());
                    sql.Append("' ");
                }

                return sql.ToString();
            }

            public static string readException(Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null) msg += "/" + ex.InnerException.Message;
                return msg;
            }

            public static void flushLogToFile(string logType, string content)
            {
                string filename = dateInfile + "log" + logType + ".txt";
                content = DateTime.Now.Date.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString() + ", " + content;

                var logger = DependencyService.Get<IWriteFile>();
                logger.WriteFile(filename, content);
            }

            public static void writeToFile(string filename, string content)
            {
                filename = dateInfile + filename;
                var baseFilename = filename.Split('.')[0];
                baseFilename = baseFilename.Replace(dateInfile, "");

                var reader = DependencyService.Get<IGetFile>();
                foreach (var file in System.IO.Directory.GetFiles(reader.GetDirectory()))
                {
                    if (file.Contains(baseFilename))
                    {
                        var remover = DependencyService.Get<IRemoveFile>();
                        remover.RemoveFile(file);
                    }
                }

                var writer = DependencyService.Get<IWriteFile>();
                writer.WriteFile(filename, content);
            }

            public async static void shareFile(string title, string filename)
            {
                filename = dateInfile + filename;
                var reader = DependencyService.Get<IGetFile>();

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = title,
                    File = new ShareFile(reader.GetFile(filename))
                });
            }
        }
    }
}
