using Acr.UserDialogs;
using doghavenCapstone.Model;
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
    public partial class AddShop : ContentPage
    {
        public static List<Label> lbl = new List<Label>();
        public static string latitude = "";
        public static string longtitude = "";
        string url = "";
        Stream shop_image = null;
        public AddShop()
        {
            InitializeComponent();
            lbl.Add(lblLocation);
            lblLocation.Text = "No location is pinned";
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Ops!", "Your device is not supported to do this function", "Okay");
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (selectedImageFile == null)
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("You haven't picked any image", new TimeSpan(2));
                    return;
                }

                imgShopPic.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                shop_image = selectedImageFile.GetStream();
            }
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {
                await DisplayAlert("Permission Error", "We need your permission to access your gallery", "Okay");
            }
        }

        private void btnAddress_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PinLostDogPage());
        }

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {
            if(txtNameShop.Text == "" || latitude == "" || longtitude == "")
            {
                DisplayAlert("Ops", "Please provide all the fields that are needed", "Okay");
            }
            else
            {
                UserDialogs.Instance.ShowLoading("Please wait while we save your information");
                uploadDogInfo(shop_image);
            }
        }

        public async void uploadDogInfo(Stream stream)
        {
            try
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
                url = blockBlob.Uri.OriginalString.ToString();
                infoInitializer();

            }
            catch (System.ArgumentNullException)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "Please select a dog image ", "Okay");
                return;
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Warning", "Something went wrong with uploading the image: ", "Okay");
                return;
            }
        }

        private void infoInitializer()
        {
            if (url != null || url != "")
            {
                uploadDogData();
            }
            else
            {
                return;
            }
        }

        private async void uploadDogData()
        {
            try
            {
                string finalDesc = "", finalLink = "";
                if(txtDescription.Text == "") { finalDesc = ""; }
                else { finalDesc = txtDescription.Text; }
                if (txtLink.Text == "") { finalLink = ""; }
                else { finalLink = txtLink.Text; }
                dogRelatedEstablishments shopInfo = new dogRelatedEstablishments()
                {

                    id = Guid.NewGuid().ToString("N").Substring(0, 30),
                    shopImage = url,
                    nameOfShop = txtNameShop.Text,
                    latitude = latitude,
                    longtitude = longtitude,
                    rate = "0",
                    addtionalDesc = finalDesc,
                    facebookLink = finalLink
                };

                await App.client.GetTable<dogRelatedEstablishments>().InsertAsync(shopInfo);
                await DisplayAlert("Confirmation", "Dog succesfully saved!", "Okay");
            }
            catch (Microsoft.WindowsAzure.MobileServices.MobileServiceInvalidOperationException ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "Please provide all the fields", "Okay");
                return;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "Please provide all the fields", "Okay");
                return;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "An error has occured: " + ex.Message, "Okay");
                return;
            }
            UserDialogs.Instance.HideLoading();
            //App.loadingMessage = "";
        }
    }
}