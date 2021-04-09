using doghavenCapstone.MainPages;
using doghavenCapstone.Model;
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
        string url = "";
        
        List<dogBreed> lstOfBreeds = new List<dogBreed>();
        List<dogPurpose> lstOfPurpose = new List<dogPurpose>();
        public UploadDogPage()
        {
            InitializeComponent();

            var assembly = typeof(UploadDogPage);
            loadBreeds();
            loadPurposeData();
            imgDogImage.Source = ImageSource.FromResource("doghavenCapstone.Assets.no_image_available.jpg", assembly);
            App.loadingMessage = "Dog information is being saved, please wait ...";
            pckrGender.Items.Add("Male");
            pckrGender.Items.Add("Female");
        }

        public async void loadBreeds()
        {
            //load breeds
            var dogBreedTable = await App.client.GetTable<dogBreed>().ToListAsync();
            foreach (var breed in dogBreedTable)
            {
                lstOfBreeds.Add(breed);
                pckrDogBreed.Items.Add(breed.breedName);
            }
        }

        public void btnUploadImage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Loading());
            uploadDogInfo(dog_image);
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
            catch (Exception)
            {
                await DisplayAlert("Warning", "Something went wrong with uploading the image", "Okay");
                return;
            }
        }

        public void infoInitializer()
        {
            if(url != null || url != "")
            {
                uploadDogData();
            }
            else
            {
                return;
            }
        }

        public async void uploadDogData()
        {
            try
            {
                if (txtDogName.Text == "" || txtDogName == null || 
                    pckrDogBreed == null || pckrDogPurpose == null ||
                    pckrGender == null || imgDogImage == null)
                {
                    await Navigation.PushAsync(new UploadDogPage());
                    await DisplayAlert("Ops!", "Please provide info to all the provided fields", "Okay");
                }
                else
                {
                    var temp_id = lstOfBreeds.FindIndex(breed => breed.breedName == pckrDogBreed.Items[pckrDogBreed.SelectedIndex]);
                    var pur_id = lstOfPurpose.FindIndex(pur => pur.dogDesc == pckrDogPurpose.Items[pckrDogPurpose.SelectedIndex]);
                    string breeed_id = lstOfBreeds[temp_id].id, purposeid = lstOfPurpose[pur_id].id;
                    dogInfo dogInformation = new dogInfo()
                    {
                        id = Guid.NewGuid().ToString("N").Substring(0,20),
                        dogName = txtDogName.Text,
                        dogGender = pckrGender.Items[pckrGender.SelectedIndex].ToString(),
                        dogImage = url,
                        dogBreed_id = breeed_id,
                        dogPurpose_id = purposeid,
                        userid = App.user_id
                    };

                    await App.client.GetTable<dogInfo>().InsertAsync(dogInformation);
                    await Navigation.PushAsync(new UploadDogPage());
                    await DisplayAlert("Confirmation", "Dog information succesfully uploaded", "Okay");
                }
            }
            catch (Exception)
            {
                await Navigation.PushAsync(new UploadDogPage());
                await DisplayAlert("Ops", "Connection timed out", "Okay");
                return;
            }
            App.loadingMessage = "";
        }

        public async void loadPurposeData()
        {
            var purposeTable = await App.client.GetTable<dogPurpose>().ToListAsync();
            foreach (var purpose in purposeTable)
            {
                lstOfPurpose.Add(purpose);
                pckrDogPurpose.Items.Add(purpose.dogDesc);
            }
        }

        public void clearData()
        {
            txtDogName.Text = "";
            pckrDogPurpose.Items.Clear();
            pckrDogBreed.Items.Clear();
            pckrGender.Items.Clear();
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BreedMatchingPage());
        }
    }
}