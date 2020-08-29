using Xamarin.Forms;
using xamarinTestBL;
using Xamarin.Essentials;

namespace xamarinTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (views.database.databaseExists())
            {
                // update
                // entities.database.resetDatabase();
            }
            else
                entities.database.initializeDatabase();

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
