using Xamarin.Forms;
using xamarinTestBL;

namespace xamarinTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (views.database.databaseExists())
            {

            }

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
