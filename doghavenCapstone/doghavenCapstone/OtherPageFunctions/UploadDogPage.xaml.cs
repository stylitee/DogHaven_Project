using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
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
using Xamarin.Essentials;
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
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var assembly = typeof(UploadDogPage);
            loadBreeds();
            loadPurposeData();
            imgDogImage.Source = ImageSource.FromResource("doghavenCapstone.Assets.no_image_available.jpg", assembly);
            pckrGender.Items.Add("Male");
            pckrGender.Items.Add("Female");
            setUserRoleInfo();
            UserDialogs.Instance.HideLoading();
        }

        private async void setUserRoleInfo()
        {
            var userinfo = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            var filter = await App.client.GetTable<userRole>().Where(x => x.id == userinfo[0].user_role_id).ToListAsync();

            if (filter[0].roleDescription == "Breeder")
            {
                int index = lstOfPurpose.FindIndex(pur => pur.dogDesc == "Breeding");
                pckrDogPurpose.SelectedIndex = index;
            }
            else if(filter[0].roleDescription == "Seller")
            {
                int index = lstOfPurpose.FindIndex(pur => pur.dogDesc == "Sale");
                pckrDogPurpose.SelectedIndex = index;
            }
            else
            {

            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
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
                
                if (txtDogName.Text == "" || txtDogName == null ||
                    pckrDogBreed == null || pckrDogPurpose == null ||
                    pckrGender == null || imgDogImage == null || url == null)
                {
                    txtDogName.Text = "";
                    DisplayAlert("Ops!", "Please provide info to all the provided fields", "Okay");
                }
                else
                {
                    UserDialogs.Instance.ShowLoading("Information is being processed, please wait");
                    uploadDogInfo(dog_image);
                }
                
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new HomeFlyOut());
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
                var temp_id = lstOfBreeds.FindIndex(breed => breed.breedName == pckrDogBreed.Items[pckrDogBreed.SelectedIndex]);
                var pur_id = lstOfPurpose.FindIndex(pur => pur.dogDesc == pckrDogPurpose.Items[pckrDogPurpose.SelectedIndex]);
                string breeed_id = lstOfBreeds[temp_id].id, purposeid = lstOfPurpose[pur_id].id;
                dogInfo dogInformation = new dogInfo()
                {
                    id = Guid.NewGuid().ToString("N").Substring(0, 20),
                    dogName = txtDogName.Text,
                    dogImage = url,
                    dogGender = pckrGender.Items[pckrGender.SelectedIndex].ToString(),
                    dogBreed_id = breeed_id,
                    dogPurpose_id = purposeid,
                    userid = App.user_id
                };

                await App.client.GetTable<dogInfo>().InsertAsync(dogInformation);
                txtDogName.Text = "";
                await DisplayAlert("Confirmation", "Dog information succesfully uploaded", "Okay");
            }
            catch (Exception ex)
            {
                txtDogName.Text = "";
                imgDogImage = null;
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ops", "An error has occured: " + ex.Message, "Okay");
                return;
            }
            txtDogName.Text = "";
            pckrDogBreed.SelectedIndex = -1;
            pckrDogPurpose.SelectedIndex = -1;
            pckrGender.SelectedIndex = -1;
            UserDialogs.Instance.HideLoading();
        }

        public async void loadPurposeData()
        {
            var purposeTable = await App.client.GetTable<dogPurpose>().ToListAsync();
            foreach (var purpose in purposeTable)
            {
                if(purpose.dogDesc != "Adoption")
                {
                    lstOfPurpose.Add(purpose);
                    pckrDogPurpose.Items.Add(purpose.dogDesc);
                }
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
                Application.Current.MainPage = new NavigationPage(new HomeFlyOut());
                Navigation.PopToRootAsync();
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new HomeFlyOut());
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

                imgDogImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                dog_image = selectedImageFile.GetStream();
            }
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {
                await DisplayAlert("Permission Error", "We need your permission to access your gallery", "Okay");
            }

        }
    }
}