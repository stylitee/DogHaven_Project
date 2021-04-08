using doghavenCapstone.PreventerPage;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
        Stream dog_image = null;

        public UploadDogPage()
        {
            InitializeComponent();

            var assembly = typeof(UploadDogPage);

            imgDogImage.Source = ImageSource.FromResource("doghavenCapstone.Assets.no_image_available.jpg", assembly);
            App.loadingMessage = "Image is being uploaded, please wait ...";
        }

        public void btnUploadImage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Loading());
            uploadDogImage(dog_image);
            App.loadingMessage = "";
            Navigation.PushAsync(new UploadDogPage());
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

            dog_image = selectedImageFile.GetStream();
        }

        public async void uploadDogImage(Stream stream)
        {
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=doghaven2storage;" +
                                                    "AccountKey=Arp3X8RJ/LG4FMED/DqRMkWJn5Ba0IUhEdTJak6z5yOHcIsx+" +
                                                    "97bgwfjuQNBLgmpt+0J0mjK8rcCGeeMJ/FT0Q==;EndpointSuffix=core.w" +
                                                    "indows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("dogimagescontainer");

            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{name}.jpg");
            await blockBlob.UploadFromStreamAsync(stream);

            string url = blockBlob.Uri.OriginalString;
        }
    }
}