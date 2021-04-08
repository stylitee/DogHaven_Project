using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadDogPage : ContentPage
    {
        public UploadDogPage()
        {
            InitializeComponent();

            var assembly = typeof(UploadDogPage);

            imgDogImage.Source = ImageSource.FromResource("doghavenCapstone.Assets.no_image_available.jpg", assembly);
        }

        public void btnTakePicture_Clicked(object sender, EventArgs e)
        {
            
        }

        private async void btnChooseGallery_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Ops!", "Your device is not supported to do this function", "Okay");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if(selectedImageFile == null)
            {
                await DisplayAlert("Ops","There was an error trying to get your image","Okay");
                return;
            }

            imgDogImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
        }
    }
}