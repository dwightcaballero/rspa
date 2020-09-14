using System.IO;
using xamarinTest.Droid.services;
using xamarinTestBL.services;

[assembly: Xamarin.Forms.Dependency(typeof(GetFile))]
namespace xamarinTest.Droid.services
{
    public class GetFile : IGetFile
    {
        string IGetFile.GetFile(string filename)
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, filename);
        }

        public string GetDirectory()
        {
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        }
    }
}