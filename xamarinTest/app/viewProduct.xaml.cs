using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarinTestBL;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProduct : ContentPage
    {
        dto.productDTO productDTO { get; set; }
        Guid selectedCategoryUID { get; set; }
        string searchText { get; set; }

        public ViewProduct()
        {
            InitializeComponent();

            productDTO = new dto.productDTO();
            selectedCategoryUID = Guid.Empty;
            searchText = string.Empty;

            productDTO.listProducts = views.product.getListProductsForListView();
            lvProducts.ItemsSource = productDTO.listProducts;

            productDTO.listCategories = views.category.getListCategoryForListview();
            var listCategoryNames = productDTO.listCategories.Select(cat => cat.categoryName).ToList();
            listCategoryNames.Insert(0, "NONE");
            ddlCategory.ItemsSource = listCategoryNames;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var sbar = (SearchBar)sender;
            searchText = sbar.Text.ToUpper().Trim();
            searchAndFilterListview();
        }

        private void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlCategory = (Picker)sender;

            if (ddlCategory.SelectedIndex == 0)
                ddlCategory.SelectedItem = null;

            if (ddlCategory.SelectedItem != null)
            {
                var category = productDTO.listCategories.Where(cat => cat.categoryName == ddlCategory.SelectedItem.ToString()).FirstOrDefault();
                if (category != null)
                    selectedCategoryUID = category.id;
                else
                    showMessage(false, "Category record not found!");
            }
            else
                selectedCategoryUID = Guid.Empty;

            searchAndFilterListview();
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
            productDTO.listProducts = e;
            lvProducts.ItemsSource = e;
            searchAndFilterListview();
        }

        private void showMessage(bool success, string message)
        {
            if (success)
                DisplayAlert("Success", message, "Close");
            else
                DisplayAlert("Error", message, "Close");
        }

        void searchAndFilterListview()
        {
            var newProductList = new List<views.product>();

            if (selectedCategoryUID != Guid.Empty)
                newProductList = productDTO.listProducts.Where(prod => prod.categoryUID == selectedCategoryUID).ToList();
            else 
                newProductList = productDTO.listProducts;

            if (!string.IsNullOrEmpty(searchText))
                newProductList = newProductList.Where(prod => prod.productFullName.Contains(searchText)).ToList();

            lvProducts.ItemsSource = newProductList;
        }
    }
}