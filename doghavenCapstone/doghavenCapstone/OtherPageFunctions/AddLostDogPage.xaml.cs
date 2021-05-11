using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLostDogPage : ContentPage
    {
        public static string setLocation_latitude = "", setLocation_longtitude = "";
        Stream dogImage = null;
        List<dogBreed> mylstOfBreeds = new List<dogBreed>();
        string url = "", dogInfo_ID = "";
        public AddLostDogPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            InitializeControls();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        public async void InitializeControls()
        {
            
            pickerDogGender.Items.Add("Male");
            pickerDogGender.Items.Add("Female");
            timeSetter.Time = DateTime.Now.TimeOfDay;
            dateSetter.Date = DateTime.Now.Date;
            dateSetter.MaximumDate = DateTime.Now.Date;
            var listOfBreeds = await App.client.GetTable<dogBreed>().ToListAsync();
            foreach(var c in listOfBreeds)
            {
                pckrBreed.Items.Add(c.breedName);
                mylstOfBreeds.Add(c);
            }
        }

        private void btnOpenMaps_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PinLostDogPage());
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

            dogImage = selectedImageFile.GetStream();
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if(setLocation_latitude != "" && setLocation_longtitude != "")
            {
                UserDialogs.Instance.ShowLoading("Please wait while we save your dog info");
                uploadDogInfo(dogImage);
            }
            else
            {
                DisplayAlert("Ops", "Please enter the location", "Okay");
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

        public async void uploadDogData()
        {
            try
            {
                string formattedTime = timeSetter.Time.Hours.ToString() + ":" + timeSetter.Time.Minutes.ToString();
                string fromattedDate = dateSetter.Date.ToString("MM/dd/yyyy");
                string trylang = timeSetter.Time.ToString();
                DateTime dt = DateTime.Parse(formattedTime);
                string finalTime = dt.ToString("hh:mm tt");

                var temp_id = mylstOfBreeds.FindIndex(breed => breed.breedName == pckrBreed.Items[pckrBreed.SelectedIndex]);

                string breeed_id = mylstOfBreeds[temp_id].id, purposeid = "5421dsad3";
                dogInfo_ID = Guid.NewGuid().ToString("N").Substring(0, 20);
                //lostDogID = Guid.NewGuid().ToString("N").Substring(0, 19);
                App.uploadFlag = 0;
                dogInfo dogInformation = new dogInfo()
                {

                    id = dogInfo_ID,
                    dogName = txtDogName.Text,
                    dogImage = url,
                    dogGender = pickerDogGender.Items[pickerDogGender.SelectedIndex].ToString(),
                    dogBreed_id = breeed_id,
                    dogPurpose_id = purposeid,
                    userid = App.user_id
                };

                await App.client.GetTable<dogInfo>().InsertAsync(dogInformation);
                //await Navigation.PushAsync(new UploadDogPage());

                //imgDogImage = null;
                LostDogs lostdogs = new LostDogs()
                {
                    id = Guid.NewGuid().ToString("N").Substring(0, 20),
                    userid = App.user_id,
                    lastSeen_date = fromattedDate,
                    lastSeen_time = finalTime,
                    placeLost_latitude = setLocation_latitude,
                    placeLost_longtitude = setLocation_longtitude,
                    dogInfo_id = dogInfo_ID
                };

                await App.client.GetTable<LostDogs>().InsertAsync(lostdogs);
                App.uploadFlag = 0;
                await DisplayAlert("Confirmation", "Dog succesfully saved!", "Okay");
            }
            catch (Microsoft.WindowsAzure.MobileServices.MobileServiceInvalidOperationException ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "Please provide all the fields", "Okay");
                return;
            }
            catch(System.ArgumentOutOfRangeException)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "Please provide all the fields", "Okay");
                return;
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
            pckrBreed.SelectedIndex = -1;
            pickerDogGender.SelectedIndex = -1;
            UserDialogs.Instance.HideLoading();
            //App.loadingMessage = "";
        }
        protected override void OnAppearing()
        {
            App.uploadFlag = 0;
            base.OnAppearing();

        }
    }
}