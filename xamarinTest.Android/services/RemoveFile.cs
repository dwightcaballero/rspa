using System.IO;
using xamarinTest.Droid.services;
using xamarinTest.services;

[assembly: Xamarin.Forms.Dependency(typeof(RemoveFile))]
namespace xamarinTest.Droid.services
{
    public class RemoveFile : IRemoveFile
    {
        void IRemoveFile.RemoveFile(string source)
        {
            File.Delete(source);
        }
    }
}