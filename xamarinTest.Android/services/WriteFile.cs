using System.IO;
using xamarinTest.Droid.services;
using xamarinTestBL.services;

[assembly: Xamarin.Forms.Dependency(typeof(WriteFile))]
namespace xamarinTest.Droid.services
{
    public class WriteFile : IWriteFile
    {
        void IWriteFile.WriteFile(string filename, string content)
        {
            string filepath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, filename);

            if (File.Exists(filepath))
                using (StreamWriter sw = new StreamWriter(filepath, true))
                    sw.WriteLine(content);
            else
                using (StreamWriter sw = new StreamWriter(filepath))
                    sw.WriteLine(content);
        }
    }
}