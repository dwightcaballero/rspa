using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarinTestBL;

namespace xamarinTest.app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategory : ContentPage
    {
        public dto.categoryDTO categoryDTO;
        public string imagePath;
        public bool isNewRecord;

        public AddCategory(views.category category)
        {
            InitializeComponent();
            categoryDTO = new dto.categoryDTO();

            if (category != null)
            {
                isNewRecord = false;
                lblTitle.Text = "View Category";
                categoryDTO.category = category;
                imgProductImage.Source = category.categoryImage;
                txtCategoryCode.Text = category.categoryCode;
                txtCategoryName.Text = category.categoryName;
                imagePath = category.categoryImage;

                btnAddImage.IsVisible = false;
                btnSelectImage.IsVisible = false;
                txtCategoryName.IsEnabled = false;
                btnEdit.IsVisible = true;
                btnSave.IsVisible = false;
            }
            else
            {
                isNewRecord = true;
                lblTitle.Text = "Add Category";
                categoryDTO.codeReference = views.codeReference.getCodeReference((int)system.sysConst.codeReferenceType.Category);
                txtCategoryCode.Text = categoryDTO.codeReference.code_string;

                btnAddImage.IsVisible = true;
                btnSelectImage.IsVisible = true;
                txtCategoryName.IsEnabled = true;
                btnEdit.IsVisible = false;
                btnSave.IsVisible = true;
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
                // save
                if (isNewRecord)
                {
                    var newCategory = new views.category();

                    newCategory.id = Guid.NewGuid();
                    newCategory.categoryCode = txtCategoryCode.Text;
                    newCategory.categoryName = system.sysTool.CleanString(txtCategoryName.Text).Trim().ToUpper();
                    newCategory.categoryImage = imagePath;
                    newCategory.createdDate = DateTime.Now;
                    newCategory.editedDate = DateTime.Now;
                    newCategory.updateType = 1;
                    categoryDTO.category = newCategory;

                    entities.category.saveCategory(categoryDTO.category);
                    entities.codeReference.updateCodeReference(categoryDTO.codeReference);

                    showMessage(true, "Successfully added a new category (" + categoryDTO.category.categoryName + ") !");
                }
                else
                {
                    categoryDTO.category.categoryName = system.sysTool.CleanString(txtCategoryName.Text).Trim().ToUpper();
                    categoryDTO.category.categoryImage = imagePath;
                    categoryDTO.category.editedDate = DateTime.Now;
                    entities.category.updateCategory(categoryDTO.category);

                    showMessage(true, "Successfully updated a category (" + categoryDTO.category.categoryName + ") !");
                }
                
                Navigation.PopAsync();
            }
        }

        private bool noValidationErrors()
        {
            // product name
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                lblCategoryName.TextColor = Color.Red;
                showMessage(false, "Category Name should not be blank.");
                return false;
            }
            else
            {
                lblCategoryName.TextColor = Color.Black;
                return true;
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

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            lblTitle.Text = "Edit Category";
            btnAddImage.IsVisible = true;
            btnSelectImage.IsVisible = true;
            txtCategoryName.IsEnabled = true;
            btnEdit.IsVisible = false;
            btnSave.IsVisible = true;
        }
    }
}