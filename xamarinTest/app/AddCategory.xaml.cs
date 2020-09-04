using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarinTest.services;
using xamarinTestBL;

namespace xamarinTest.app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategory : ContentPage
    {
        public dto.categoryDTO categoryDTO;
        public string imagePath;
        public bool isNewRecord;
        public event EventHandler<List<views.category>> updateCategoryList;

        public AddCategory(Guid categoryUID)
        {
            InitializeComponent();
            categoryDTO = new dto.categoryDTO();

            if (categoryUID != Guid.Empty)
            {
                var category = views.category.getCategoryByID(categoryUID);
                if (category != null)
                {
                    isNewRecord = false;
                    categoryDTO.category = category;
                    populatePage();
                }
                else showMessage(false, "Category record not found!");

                showControls("view");
            }
            else
            {
                isNewRecord = true;
                categoryDTO.codeReference = views.codeReference.getCodeReference((int)system.sysConst.codeReferenceType.Category);
                txtCategoryCode.Text = categoryDTO.codeReference.code_string;

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
                //gather
                var newCategory = new views.category();

                newCategory.id = Guid.NewGuid();
                newCategory.categoryCode = txtCategoryCode.Text;
                newCategory.categoryName = system.sysTool.CleanString(txtCategoryName.Text).Trim().ToUpper();
                newCategory.categoryImage = imagePath;
                newCategory.createdDate = DateTime.Now;
                newCategory.editedDate = DateTime.Now;
                newCategory.updateType = 1;

                // save
                if (isNewRecord)
                {
                    categoryDTO.category = newCategory;

                    entities.category.saveCategory(categoryDTO.category);
                    entities.codeReference.updateCodeReference(categoryDTO.codeReference);

                    showMessage(true, "Successfully added a new category (" + categoryDTO.category.categoryName + ") !");
                }
                else
                {
                    categoryDTO.category.categoryName = newCategory.categoryName;
                    categoryDTO.category.categoryImage = imagePath;
                    categoryDTO.category.editedDate = DateTime.Now;
                    entities.category.updateCategory(categoryDTO.category);

                    showMessage(true, "Successfully updated a category (" + categoryDTO.category.categoryName + ") !");
                }

                updateCategoryList?.Invoke(this, views.category.getListCategoryForListview());
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

        private void showMessage(bool success, string message)
        {
            if (success)
                DisplayAlert("Success", message, "Close");
            else
                DisplayAlert("Error", message, "Close");
        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            showControls("edit");
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var discard = await DisplayAlert("Warning", "Are you sure you want to delete the category record (" + categoryDTO.category.categoryName + ")?", "Yes", "No");
            if (discard)
            {
                entities.category.deleteCategory(categoryDTO.category);
                updateCategoryList?.Invoke(this, views.category.getListCategoryForListview());
                showMessage(true, "Category record (" + categoryDTO.category.categoryName + ") successfully removed!");
                await Navigation.PopAsync();
            }
        }

        private void showControls(string type)
        {
            switch (type)
            {
                case "add":
                    lblTitle.Text = "Add Category";
                    btnAddImage.IsVisible = true;
                    btnSelectImage.IsVisible = true;
                    txtCategoryName.IsEnabled = true;
                    btnEdit.IsVisible = false;
                    btnDelete.IsVisible = false;
                    btnSave.IsVisible = true;
                    btnRemoveImage.IsVisible = false;
                    break;
                case "view":
                    lblTitle.Text = "View Category";
                    btnAddImage.IsVisible = false;
                    btnSelectImage.IsVisible = false;
                    txtCategoryName.IsEnabled = false;
                    btnEdit.IsVisible = true;
                    btnDelete.IsVisible = true;
                    btnSave.IsVisible = false;
                    btnRemoveImage.IsVisible = false;
                    break;
                case "edit":
                    lblTitle.Text = "Edit Category";
                    btnAddImage.IsVisible = true;
                    btnSelectImage.IsVisible = true;
                    txtCategoryName.IsEnabled = true;
                    btnEdit.IsVisible = false;
                    btnDelete.IsVisible = false;
                    btnSave.IsVisible = true;
                    if (!string.IsNullOrEmpty(imagePath)) btnRemoveImage.IsVisible = true;
                    break;
            }
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
                    populatePage();
                    showControls("view");
                }
                else await Navigation.PopAsync();
            }
        }

        private void populatePage()
        {
            imgProductImage.Source = categoryDTO.category.categoryImage;
            txtCategoryCode.Text = categoryDTO.category.categoryCode;
            txtCategoryName.Text = categoryDTO.category.categoryName;
            imagePath = categoryDTO.category.categoryImage;
        }
    }
}