using System.IO;
using xamarinTest.Droid.services;
using xamarinTestBL.services;

[assembly: Xamarin.Forms.Dependency(typeof(RemoveFile))]
namespace xamarinTest.Droid.services
{
    public class RemoveFile : IRemoveFile
    {
        void IRemoveFile.RemoveFile(string filename)
        {
            string filepath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, filename);
            File.Delete(filepath);
        }
    }
}