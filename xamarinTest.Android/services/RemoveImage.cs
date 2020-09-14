using System.IO;
using xamarinTest.Droid.services;
using xamarinTest.services;

[assembly: Xamarin.Forms.Dependency(typeof(RemoveImage))]
namespace xamarinTest.Droid.services
{
    public class RemoveImage : IRemoveImage
    {
        void IRemoveImage.RemoveImage(string source)
        {
            File.Delete(source);
        }


    }
}