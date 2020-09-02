using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarinTestBL;

namespace xamarinTest.app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPriceHistory : ContentPage
    {
        public ViewPriceHistory(Guid productUID)
        {
            InitializeComponent();

            var product = views.product.getProductByID(productUID);
            var listPriceHistory = views.priceHistory.GetListPriceHistoryForListview(productUID);

            lblTitleProduct.Text = product.productBrand + " " + product.productName + " " + product.productVariation;
            lvPriceHistory.ItemsSource = listPriceHistory;
        }
    }
}