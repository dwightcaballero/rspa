using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            checkPermissions();

            VersionTracking.Track();

            lblVersion.Text = "RSPA v" + VersionTracking.CurrentVersion;
        }

        private async void checkPermissions()
        {
            while (true)
            {
                // check permissions
                var status1 = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                var status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                var status3 = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status1 != PermissionStatus.Granted)
                    status1 = await Permissions.RequestAsync<Permissions.StorageRead>();

                if (status2 != PermissionStatus.Granted)
                    status2 = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (status3 != PermissionStatus.Granted)
                    status3 = await Permissions.RequestAsync<Permissions.Camera>();

                if (status1 == PermissionStatus.Granted & status2 == PermissionStatus.Granted & status3 == PermissionStatus.Granted)
                    break;
            }
        }

        private async void btnViewProduct_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ViewProduct());
        }

        private async void btnAddProduct_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddProductGrocery(Guid.Empty));
        }

        private async void btnViewCategory_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ViewCategory());
        }
    }
}