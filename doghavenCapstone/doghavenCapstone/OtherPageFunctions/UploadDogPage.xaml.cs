using Acr.UserDialogs;
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
            pckrGender.Items.Add("Male");
            pckrGender.Items.Add("Female");
            UserDialogs.Instance.HideLoading();
        }

        protected override void OnAppearing()
        {
            App._updateflag = false;
            App.uploadFlag = 0;
            base.OnAppearing();
            btnBack.Text = App.buttonName;
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
            if(App.buttonName == "Proceed")
            {
                //Application.Current.MainPage = new HomeFlyOut();
                UserDialogs.Instance.ShowLoading("Information is being processed, please wait");
                uploadDogInfo(dog_image);
                
            }
            else
            {
                Application.Current.MainPage = new HomeFlyOut();
                UserDialogs.Instance.ShowLoading("Information is being processed, please wait");
                uploadDogInfo(dog_image);
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
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
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
                    //clear input
                    txtDogName.Text = "";
                    //imgDogImage = null;
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
                        dogImage = url,
                        dogGender = pckrGender.Items[pckrGender.SelectedIndex].ToString(),
                        dogBreed_id = breeed_id,
                        dogPurpose_id = purposeid,
                        userid = App.user_id
                    };

                    await App.client.GetTable<dogInfo>().InsertAsync(dogInformation);
                    //await Navigation.PushAsync(new UploadDogPage());
                    txtDogName.Text = "";
                    //imgDogImage = null;
                    await DisplayAlert("Confirmation", "Dog information succesfully uploaded", "Okay");
                }
            }
            catch (Exception ex)
            {
                txtDogName.Text = "";
                imgDogImage = null;
                //await Navigation.PushAsync(new UploadDogPage());
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "An error has occured: " + ex.Message, "Okay");
                return;
            }
            //imgDogImage.Source = null;
            txtDogName.Text = "";
            pckrDogBreed.SelectedIndex = -1;
            pckrDogPurpose.SelectedIndex = -1;
            pckrGender.SelectedIndex = -1;
            UserDialogs.Instance.HideLoading();
            //App.loadingMessage = "";
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
            if(btnBack.Text == "Back")
            {
                Application.Current.MainPage = new HomeFlyOut();
                Navigation.PopToRootAsync();
            }
            else
            {
                Application.Current.MainPage = new HomeFlyOut();
                Navigation.PushAsync(new HomeFlyOut());
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
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

            imgDogImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

            dog_image = selectedImageFile.GetStream();
        }
    }
}