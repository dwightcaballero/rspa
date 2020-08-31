using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamarinTest.app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPriceHistory : ContentPage
    {
        public ViewPriceHistory(Guid productUID)
        {
            InitializeComponent();
        }
    }
}