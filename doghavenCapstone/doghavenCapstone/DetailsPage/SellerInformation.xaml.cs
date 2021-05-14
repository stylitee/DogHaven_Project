using Acr.UserDialogs;
using doghavenCapstone.InitialPages;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
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

namespace doghavenCapstone.DetailsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerInformation : ContentPage
    {
        string document_one = "", document_two = "";
        Stream document_ones = null, document_twos = null;
        public SellerInformation()
        {
            InitializeComponent();
        }



        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            if(document_ones != null && document_twos != null)
            {
                uploadDocuments(document_ones, document_one);
                uploadDocuments(document_twos, document_two);
                uploadData();
            }
            else
            {
                DisplayAlert("Ops", "Please upload your documents", "Okay");
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            if(App.flagForSellerApplication == "ChangeUserType")
            {
                Navigation.PushAsync(new ChangeUserTypePage());
            }
            else
            {
                Navigation.PopAsync();
            }
            
        }

        private async void uploadDocuments(Stream dogImage, string documentnumber)
        {
            try
            {
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=doghaven2storage;" +
                                                    "AccountKey=Arp3X8RJ/LG4FMED/DqRMkWJn5Ba0IUhEdTJak6z5yOHcIsx+" +
                                                    "97bgwfjuQNBLgmpt+0J0mjK8rcCGeeMJ/FT0Q==;EndpointSuffix=core.w" +
                                                    "indows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("dogsellerapplicationrequest");

                await container.CreateIfNotExistsAsync();

                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.jpg");

                await blockBlob.UploadFromStreamAsync(dogImage);
                documentnumber = blockBlob.Uri.OriginalString.ToString();

            }
            catch (System.ArgumentNullException ex)
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

                imgValidID.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                document_ones = selectedImageFile.GetStream();
            }
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {
                await DisplayAlert("Ops", "We need you permission to access your photos", "Okay");
            }
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
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

                imgValidID.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                document_twos = selectedImageFile.GetStream();
            }
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {
                await DisplayAlert("Ops", "We need you permission to access your photos", "Okay");
            }
        }


        private async void uploadData()
        {
            try
            {
                SellerAdminRequest sell = new SellerAdminRequest()
                {
                    id = Guid.NewGuid().ToString("N").Substring(0, 16),
                    user_id = App.user_id,
                    valid_id = document_one,
                    licence_id = document_two
                };

                await App.client.GetTable<SellerAdminRequest>().InsertAsync(sell);
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "SellerAdminRequest Error" + e.Message, "Okay");
            }
        }
    }
}