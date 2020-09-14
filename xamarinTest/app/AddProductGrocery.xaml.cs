using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using xamarinTestBL;
using System.Collections.Generic;
using System.Linq;
using xamarinTest.services;
using xamarinTest.app;
using Xamarin.Forms.Markup;

namespace xamarinTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductGrocery : ContentPage
    {
        public dto.productDTO productDTO;
        public string imagePath;
        public bool isNewRecord;
        public event EventHandler<List<views.product>> updateProductList;

        public AddProductGrocery(Guid productUID)
        {
            InitializeComponent();
            productDTO = new dto.productDTO();

            productDTO.listCategories = views.category.getListCategoryForListview();
            var listCategoryNames = productDTO.listCategories.Select(cat => cat.categoryName).ToList();
            ddlCategory.ItemsSource = listCategoryNames;

            if (productUID != Guid.Empty)
            {
                var product = views.product.getProductByID(productUID);
                if (product != null)
                {
                    isNewRecord = false;
                    productDTO.product = product;
                    populatePage();
                }
                else showMessage(false, "Product Record not found!");

                showControls("view");
            }
            else
            {
                isNewRecord = true;
                productDTO.codeReference = views.codeReference.getCodeReference((int)system.sysConst.codeReferenceType.Product);
                txtProductCode.Text = productDTO.codeReference.code_string;

                showControls("add");
            }
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

            btnRemoveImage.IsVisible = true;
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

            btnRemoveImage.IsVisible = true;
        }

        private void btnRemoveImage_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IRemoveFile>().RemoveFile(imagePath);
            btnRemoveImage.IsVisible = false;
            imagePath = string.Empty;
            imgProductImage.Source = string.Empty;
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if (noValidationErrors())
            {
                // gather
                var newProduct = new views.product();

                newProduct.id = Guid.NewGuid();
                newProduct.productCode = productDTO.codeReference.code_string;
                newProduct.productName = txtProductName.Text.Trim().ToUpper();
                newProduct.productImage = imagePath;
                newProduct.createdDate = DateTime.Now;
                newProduct.editedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(txtProductBrand.Text))
                    newProduct.productBrand = txtProductBrand.Text.Trim().ToUpper();
                else
                    newProduct.productBrand = string.Empty;

                if (!string.IsNullOrEmpty(txtVariation.Text))
                    newProduct.productVariation = txtVariation.Text.Trim().ToUpper();
                else
                    newProduct.productVariation = string.Empty;

                if (!string.IsNullOrEmpty(txtStore.Text))
                    newProduct.productStore = txtStore.Text.Trim().ToUpper();
                else
                    newProduct.productStore = string.Empty;

                // category
                if (ddlCategory.SelectedItem != null)
                {
                    var selectedCategory = productDTO.listCategories.Where(cat => cat.categoryName == ddlCategory.SelectedItem.ToString()).FirstOrDefault();
                    if (selectedCategory != null)
                        newProduct.categoryUID = selectedCategory.id;
                    else
                        newProduct.categoryUID = Guid.Empty;
                }
                else newProduct.categoryUID = Guid.Empty;

                // recompute price (by piece) if piece or pack is not 1
                var price = Convert.ToDecimal(txtPrice.Text);
                newProduct.productPrice_Initial = price;

                decimal quantityPack = 1;
                decimal quantityPiece = 1;

                if (!string.IsNullOrWhiteSpace(txtQuantityPack.Text))
                {
                    quantityPack = Convert.ToDecimal(txtQuantityPack.Text);

                    quantityPiece = 1;
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
                        quantityPiece = Convert.ToDecimal(txtQuantityPiece.Text);
                        if (quantityPiece > 1)
                            price /= quantityPiece;
                    }
                }

                newProduct.productPack_Initial = quantityPack;
                newProduct.productPiece_Initial = quantityPiece;
                newProduct.productPrice = price;
                newProduct.updateType = 1;

                var priceHistory = new views.priceHistory();
                priceHistory.id = Guid.NewGuid();
                priceHistory.price = newProduct.productPrice;
                priceHistory.store = newProduct.productStore;
                priceHistory.updateType = 1;
                priceHistory.loggedDate = DateTime.Now;

                if (isNewRecord)
                {
                    productDTO.product = newProduct;
                    priceHistory.productUID = newProduct.id;

                    // save
                    entities.product.saveProduct(productDTO.product);
                    entities.codeReference.updateCodeReference(productDTO.codeReference);
                    entities.priceHistory.addPriceHistory(priceHistory);
                    showMessage(true, "Successfully added a new product (" + productDTO.product.productName + ") !");
                }
                else
                {
                    productDTO.product.productName = newProduct.productName;
                    productDTO.product.productImage = imagePath;
                    productDTO.product.productBrand = newProduct.productBrand;
                    productDTO.product.productVariation = newProduct.productVariation;
                    productDTO.product.productStore = newProduct.productStore;
                    productDTO.product.categoryUID = newProduct.categoryUID;
                    productDTO.product.productPrice = newProduct.productPrice;
                    productDTO.product.productPrice_Initial = newProduct.productPrice_Initial;
                    productDTO.product.productPiece_Initial = newProduct.productPiece_Initial;
                    productDTO.product.productPack_Initial = newProduct.productPack_Initial;
                    productDTO.product.editedDate = DateTime.Now;
                    priceHistory.productUID = productDTO.product.id;

                    entities.product.updateProduct(productDTO.product);
                    entities.priceHistory.addPriceHistory(priceHistory);
                    showMessage(true, "Successfully updated a product (" + productDTO.product.productName + ") !");
                }

                updateProductList?.Invoke(this, views.product.getListProductsForListView());
                Navigation.PopAsync();
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
                if (!decimal.TryParse(txtPrice.Text, out _))
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

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            showControls("edit");
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var discard = await DisplayAlert("Warning", "Are you sure you want to delete the product record (" + productDTO.product.productFullName + ")?", "Yes", "No");
            if (discard)
            {
                entities.product.deleteProduct(productDTO.product);
                updateProductList?.Invoke(this, views.product.getListProductsForListView());
                showMessage(true, "Product record (" + productDTO.product.productFullName + ") successfully removed!");
                await Navigation.PopAsync();
            }
        }

        private void showMessage(bool success, string message)
        {
            if (success)
                DisplayAlert("Success", message, "Close");
            else
                DisplayAlert("Error", message, "Close");
        }

        private void showControls(string type)
        {
            switch (type)
            {
                case "add":
                    lblTitle.Text = "Add Product";
                    btnAddImage.IsVisible = true;
                    btnSelectImage.IsVisible = true;
                    btnEdit.IsVisible = false;
                    btnDelete.IsVisible = false;
                    btnSave.IsVisible = true;
                    btnRemoveImage.IsVisible = false;

                    txtProductName.IsEnabled = true;
                    txtProductBrand.IsEnabled = true;
                    txtVariation.IsEnabled = true;
                    txtStore.IsEnabled = true;
                    ddlCategory.IsEnabled = true;
                    gridAddEditPrice.IsVisible = true;
                    gridViewPrice.IsVisible = false;
                    lblPrice.IsVisible = true;
                    txtPrice.IsVisible = true;
                    break;

                case "view":
                    lblTitle.Text = "View Product";
                    btnAddImage.IsVisible = false;
                    btnSelectImage.IsVisible = false;
                    btnEdit.IsVisible = true;
                    btnDelete.IsVisible = true;
                    btnSave.IsVisible = false;
                    btnRemoveImage.IsVisible = false;

                    txtProductName.IsEnabled = false;
                    txtProductBrand.IsEnabled = false;
                    txtVariation.IsEnabled = false;
                    txtStore.IsEnabled = false;
                    ddlCategory.IsEnabled = false;
                    gridAddEditPrice.IsVisible = false;
                    gridViewPrice.IsVisible = true;
                    lblPrice.IsVisible = false;
                    txtPrice.IsVisible = false;
                    break;

                case "edit":
                    lblTitle.Text = "Edit Product";
                    btnAddImage.IsVisible = true;
                    btnSelectImage.IsVisible = true;
                    btnEdit.IsVisible = false;
                    btnDelete.IsVisible = false;
                    btnSave.IsVisible = true;
                    if (!string.IsNullOrEmpty(imagePath)) btnRemoveImage.IsVisible = true;

                    txtProductName.IsEnabled = true;
                    txtProductBrand.IsEnabled = true;
                    txtVariation.IsEnabled = true;
                    txtStore.IsEnabled = true;
                    ddlCategory.IsEnabled = true;
                    gridAddEditPrice.IsVisible = true;
                    gridViewPrice.IsVisible = false;
                    lblPrice.IsVisible = true;
                    txtPrice.IsVisible = true;
                    break;
            }
        }

        private void lnkPriceHistory_Tapped(object sender, EventArgs e)
        {
            var lblPriceHistory = (Label)sender;
            var tap = (TapGestureRecognizer)lblPriceHistory.GestureRecognizers[0];
            var productUID = Guid.Parse(tap.CommandParameter.ToString());
            Navigation.PushAsync(new ViewPriceHistory(productUID));
        }

        protected override bool OnBackButtonPressed()
        {
            // discard changes made during edit
            if (btnSave.IsVisible)
                discardChanges();
            else
                Navigation.PopAsync();

            return true;
        }

        public async void discardChanges()
        {
            var discard = await DisplayAlert("Warning", "Discard all changes made?", "Yes", "No");
            if (discard)
            {
                if (!isNewRecord)
                {
                    showControls("view");
                    populatePage();
                }
                else await Navigation.PopAsync();
            }
        }

        private void populatePage()
        {
            imgProductImage.Source = productDTO.product.productImage;
            imagePath = productDTO.product.productImage;

            txtProductCode.Text = productDTO.product.productCode;
            txtProductName.Text = productDTO.product.productName;

            if (!string.IsNullOrEmpty(productDTO.product.productBrand))
                txtProductBrand.Text = productDTO.product.productBrand;
            else
                txtProductBrand.Placeholder = string.Empty;

            if (!string.IsNullOrEmpty(productDTO.product.productVariation))
                txtVariation.Text = productDTO.product.productVariation;
            else
                txtVariation.Placeholder = string.Empty;

            if (!string.IsNullOrEmpty(productDTO.product.productStore))
                txtStore.Text = productDTO.product.productStore;
            else
                txtStore.Placeholder = string.Empty;

            var selectedCategory = productDTO.listCategories.Where(cat => cat.id == productDTO.product.categoryUID).FirstOrDefault();
            if (selectedCategory != null) ddlCategory.SelectedItem = selectedCategory.categoryName;

            txtQuantityPack.Text = productDTO.product.productPack_Initial.ToString();
            txtQuantityPiece.Text = productDTO.product.productPiece_Initial.ToString();
            txtPrice.Text = productDTO.product.productPrice_Initial.ToString("N2");

            txtOriginalPrice.Text = "P " + productDTO.product.productPrice.ToString("N2");
            txt10Price.Text = "P " + productDTO.product.productPrice_10.ToString("N2");
            txt15Price.Text = "P " + productDTO.product.productPrice_15.ToString("N2");

            tapPriceHistory.CommandParameter = productDTO.product.id.ToString();
        }
    }
}