using System;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private async void btnAddImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                showMessage(false, "No camera available.");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92,
                DefaultCamera = CameraDevice.Rear,
                Directory = "images",
                Name = Guid.NewGuid().ToString() + ".jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            imgProductImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void btnSelectImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                showMessage(false, "Uploading of image is not available.");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            imgProductImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if (noValidationErrors())
            {

            }
        }

        private bool noValidationErrors()
        {
            var errorList = new StringBuilder();

            // product code
            if (string.IsNullOrWhiteSpace(txtProductCode.Text))
            {
                errorList.AppendLine("Product Code should not be blank.");
                lblProductCode.TextColor = Color.Red;
            }
            else lblProductCode.TextColor = Color.Black;

            // product name
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorList.AppendLine("Product Name should not be blank.");
                lblProductName.TextColor = Color.Red;
            }
            else lblProductName.TextColor = Color.Black;

            // quantity (pack)
            if (!string.IsNullOrWhiteSpace(txtQuantityPack.Text))
            {
                if (!int.TryParse(txtQuantityPack.Text, out _))
                {
                    errorList.AppendLine("Quantity-Pack (" + txtQuantityPack.Text + ") should be numeric.");
                    lblQuantityPack.TextColor = Color.Red;
                }
                else lblQuantityPack.TextColor = Color.Black;
            }

            // quantity (piece)
            lblQuantityPiece.TextColor = Color.Black;
            if (string.IsNullOrWhiteSpace(txtQuantityPiece.Text))
            {
                errorList.AppendLine("Quantity-Piece should not be blank.");
                lblQuantityPiece.TextColor = Color.Red;
            }
            else
            {
                if (!int.TryParse(txtQuantityPiece.Text, out _))
                {
                    errorList.AppendLine("Quantity-Piece (" + txtQuantityPiece.Text + ") should be numeric.");
                    lblQuantityPiece.TextColor = Color.Red;
                }
            }

            // price
            lblPrice.TextColor = Color.Black;
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                errorList.AppendLine("Price should not be blank.");
                lblPrice.TextColor = Color.Red;
            }
            else
            {
                if (!int.TryParse(txtPrice.Text, out _))
                {
                    errorList.AppendLine("Price (" + txtQuantityPiece.Text + ") should be numeric.");
                    lblPrice.TextColor = Color.Red;
                }
            }

            if (string.IsNullOrEmpty(errorList.ToString()))
            {
                return true;
            }
            else
            {
                showMessage(false, errorList.ToString());
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