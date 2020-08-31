using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarinTestBL;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProduct : ContentPage
    {
        dto.productDTO productDTO { get; set; }

        public ViewProduct()
        {
            InitializeComponent();

            productDTO = new dto.productDTO();
            productDTO.listProducts = views.product.getListProductsForListView();
            lvProducts.ItemsSource = productDTO.listProducts;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var sbar = (SearchBar)sender;
            var text = sbar.Text.ToUpper().Trim();
            var newProductList = productDTO.listProducts.Where(prod => 
                prod.productBrand.Contains(text) || 
                prod.productName.Contains(text) ||
                prod.productVariation.Contains(text)).ToList();
            lvProducts.ItemsSource = newProductList;
        }

        private void btnViewProduct_Clicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var product = productDTO.listProducts.Where(prod => prod.id == Guid.Parse(btn.CommandParameter.ToString())).FirstOrDefault();
            if (product != null)
            {
                var AddProductGrocery = new AddProductGrocery(product.id);
                AddProductGrocery.updateProductList += AddProductGrocery_updateProductList;
                Navigation.PushAsync(AddProductGrocery);
            }
            else showMessage(false, "Product record not found!");
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {
            var AddProductGrocery = new AddProductGrocery(Guid.Empty);
            AddProductGrocery.updateProductList += AddProductGrocery_updateProductList;
            Navigation.PushAsync(AddProductGrocery);
        }

        private void AddProductGrocery_updateProductList(object sender, List<views.product> e)
        {
            lvProducts.ItemsSource = e;
        }

        private void showMessage(bool success, string message)
        {
            if (success)
                DisplayAlert("Success", message, "Close");
            else
                DisplayAlert("Error", message, "Close");
        }
    }
}