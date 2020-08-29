using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarinTest.app;
using xamarinTestBL;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCategory : ContentPage
    {
        dto.categoryDTO categoryDTO;

        public ViewCategory()
        {
            InitializeComponent();

            categoryDTO = new dto.categoryDTO();
            categoryDTO.listCategories = views.category.getListCategory();
            lvCategories.ItemsSource = categoryDTO.listCategories;
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddCategory(null));
        }

        private void btnViewCategory_Clicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var category = categoryDTO.listCategories.Where(cat => cat.id == Guid.Parse(btn.CommandParameter.ToString())).FirstOrDefault();
            if (category != null)
            {
                Navigation.PushAsync(new AddCategory(category));
            }
            else showMessage(false, "Category record not found!");
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var sbar = (SearchBar)sender;
            var newCategoryList = categoryDTO.listCategories.Where(cat => cat.categoryName.Contains(sbar.Text.ToUpper())).ToList();
            lvCategories.ItemsSource = newCategoryList;
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