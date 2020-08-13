using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using xamarinTestBL;
using System.Collections.Generic;
using System.Linq;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductGrocery : ContentPage
    {
        public dto.productDTO productDTO;
        public string imagePath;

        public AddProductGrocery()
        {
            InitializeComponent();

            productDTO = new dto.productDTO();
            productDTO.codeReference = views.codeReference.getCodeReference();
            //txtProductCode.Text = productDTO.codeReference.productCode;
        }

        private async void btnAddImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                showMessage(false, "No camera available.");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 92,
                DefaultCamera = CameraDevice.Rear,
                Directory = "images",
                Name = Guid.NewGuid().ToString() + ".jpg"
            });

            if (file == null)
                return;

            imagePath = file.Path;

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

            imagePath = file.Path;

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
                var newProduct = new views.product();
                var listCategories = new List<views.category>(); // get list from database

                newProduct.id = Guid.NewGuid();
                newProduct.productCode = txtProductCode.Text.Trim();
                newProduct.productName = txtProductName.Text.Trim();
                newProduct.productBrand = txtProductBrand.Text.Trim();
                newProduct.productVariation = txtVariation.Text.Trim();
                newProduct.productStore = txtStore.Text.Trim();

                // category
                var selectedCategory = listCategories.Where(cat => cat.categoryName == ddlCategory.SelectedItem.ToString()).FirstOrDefault();
                if (selectedCategory != null)
                    newProduct.categoryUID = selectedCategory.id;

                // recompute price (by piece) if piece or pack is not 1
                var price = Convert.ToDecimal(txtPrice.Text);

                if (!string.IsNullOrWhiteSpace(txtQuantityPack.Text))
                {
                    var quantityPack = Convert.ToDecimal(txtQuantityPack.Text);

                    decimal quantityPiece = 1;
                    if (!string.IsNullOrWhiteSpace(txtQuantityPiece.Text))
                        quantityPiece = Convert.ToDecimal(txtQuantityPiece.Text);

                    if (quantityPack > 1)
                    {
                        price /= (quantityPack * quantityPiece);
                    }
                    else
                    {
                        if (quantityPiece > 1)
                            price /= quantityPiece;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(txtQuantityPiece.Text))
                    {
                        var quantityPiece = Convert.ToDecimal(txtQuantityPiece.Text);
                        if (quantityPiece > 1)
                            price /= quantityPiece;
                    } 
                }

                newProduct.productPrice = price;

                // save
            }
        }

        private bool noValidationErrors()
        {
            var errorList = new StringBuilder();

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
                if (!decimal.TryParse(txtQuantityPack.Text, out _))
                {
                    errorList.AppendLine("Quantity-Pack (" + txtQuantityPack.Text + ") should be numeric.");
                    lblQuantityPack.TextColor = Color.Red;
                }
                else lblQuantityPack.TextColor = Color.Black;
            }

            // quantity (piece)
            lblQuantityPiece.TextColor = Color.Black;
            if (!string.IsNullOrWhiteSpace(txtQuantityPiece.Text))
            {
                if (!decimal.TryParse(txtQuantityPiece.Text, out _))
                {
                    errorList.AppendLine("Quantity-Piece (" + txtQuantityPiece.Text + ") should be numeric.");
                    lblQuantityPiece.TextColor = Color.Red;
                }
                else lblQuantityPack.TextColor = Color.Black;
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