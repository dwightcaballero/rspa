using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void btnAddImage_Clicked(object sender, EventArgs e)
        {

        }

        private void btnSelectImage_Clicked(object sender, EventArgs e)
        {

        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if (noValidationErrors())
            {

            }
        }

        private bool noValidationErrors()
        {
            string error = string.Empty;

            // product code
            if (string.IsNullOrWhiteSpace(txtProductCode.Text))
            {

            }

            // product name
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {

            }

            // quantity (piece)
            if (string.IsNullOrWhiteSpace(txtQuantityPiece.Text))
            {

            }
            if (int.TryParse(txtQuantityPiece.Text, out _))
            {

            }

            // quantity (price)
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {

            }
            if (int.TryParse(txtPrice.Text, out _))
            {

            }

            if (string.IsNullOrEmpty(error))
            {
                return true;
            }
            else
            {
                showMessage(false, error);
                return false; 
            }
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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