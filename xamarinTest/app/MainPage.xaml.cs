using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            VersionTracking.Track();

            lblVersion.Text = "RSPA v" + VersionTracking.CurrentVersion;
        }

        private async void btnViewProduct_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ViewProduct());
        }

        private async void btnAddProduct_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddProduct());
        }

        private async void btnViewCategory_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ViewCategory());
        }
    }
}