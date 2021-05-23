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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogUpdateInfo : ContentPage
    {
        public static int flag = 0;
        
        List<dogBreed> lstofBreeds = new List<dogBreed>();
        List<dogPurpose> lstofPurpose = new List<dogPurpose>();
        string defaultImage = "";
        Stream dog_image = null;
        string url = "";
        DateTimeOffset _createdAt, _updatedAt;
        public DogUpdateInfo()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            loadPickers();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void loadPickers()
        {
            lstofPurpose.Clear();
            lstofBreeds.Clear();
            pckrBreed.Items.Clear();
            pckrGender.Items.Clear();
            pckrPurpose.Items.Clear();
            var doginfo = await App.client.GetTable<dogPurpose>().ToListAsync();
            var breedinfo = await App.client.GetTable<dogBreed>().ToListAsync();
            pckrGender.Items.Add("Male");
            pckrGender.Items.Add("Female");

            foreach(var info in breedinfo)
            {
                pckrBreed.Items.Add(info.breedName);
                lstofBreeds.Add(info);
            }

            foreach (var info in doginfo)
            {
                pckrPurpose.Items.Add(info.dogDesc);
                lstofPurpose.Add(info);
            }

            if(pckrPurpose.Items.Count == doginfo.Count && pckrBreed.Items.Count == breedinfo.Count)
            {
                loadInformation();
            }
        }

        private async void loadInformation()
        {
            var doginfo = await App.client.GetTable<dogInfo>().Where(x => x.id == App.dog_id).ToListAsync();
            var breedinfo = await App.client.GetTable<dogBreed>().Where(x => x.id == doginfo[0].dogBreed_id).ToListAsync();
            var purposeinfo = await App.client.GetTable<dogPurpose>().Where(x => x.id == doginfo[0].dogPurpose_id).ToListAsync();
            txtName.Text = doginfo[0].dogName;
            pckrGender.SelectedItem = doginfo[0].dogGender;
            pckrBreed.SelectedItem = breedinfo[0].breedName;
            pckrPurpose.SelectedItem = purposeinfo[0].dogDesc;
            imgDog.Source = doginfo[0].dogImage;
            defaultImage = doginfo[0].dogImage;
            _createdAt = doginfo[0].createdAt;
            _updatedAt = doginfo[0].updatedAt;
        }

        protected override void OnAppearing()
        {
            App._updateflag = false;
            flag = 1;
            App.uploadFlag = 0;
            App.doginfo_flag = 1;
            base.OnAppearing();
        }

        private void btnUpdate_Clicked(object sender, EventArgs e)
        {
            App.doginfo_flag = 1;
            
            if (dog_image != null)
            {
                uploadUserImage(dog_image);
            }
            else
            {
                saveInformation();
            }
        }

        public void saveInformation()
        {
            try
            {
                string _dogimage = "";
                if (url != "")
                {
                    _dogimage = url;
                }
                else if (url == "")
                {
                    _dogimage = defaultImage;
                }

                var index = lstofBreeds.FindIndex(breed => breed.breedName ==
                                                             pckrBreed.Items[pckrBreed.SelectedIndex]);
                var breed_id = lstofBreeds[index].id;
                var i = lstofPurpose.FindIndex(purpose => purpose.dogDesc ==
                                                              pckrPurpose.Items[pckrPurpose.SelectedIndex]);
                var purpose_id = lstofPurpose[i].id;
                string _gender = pckrGender.Items[pckrGender.SelectedIndex].ToString();
                if (txtName.Text != "")
                {
                    
                    dogInfo info = new dogInfo()
                    {
                        id = App.dog_id,
                        dogName = txtName.Text,
                        dogImage = _dogimage,
                        dogGender = _gender,               
                        dogBreed_id = breed_id,
                        dogPurpose_id = purpose_id,
                        userid = App.user_id,
                        createdAt = _createdAt,
                        updatedAt = _updatedAt
                    };
                    App._updateflag = false;
                    dogInfo.Update(info);
                    
                    DisplayAlert("Confirmation", "Dog is succesfully updated", "Okay");                
                    flag = 0;
                }
                else
                {
                    DisplayAlert("Ops!", "Some fields cannot be empty", "Okay");
                }

            }
            catch (Exception e)
            {
                DisplayAlert("error", e.Message, "okay");
            }
            App.doginfo_flag = 0;
            App._updateflag = true;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            lstofBreeds.Clear();
            lstofPurpose.Clear();
        }

        private async void imgDog_Tapped(object sender, EventArgs e)
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

            imgDog.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

            dog_image = selectedImageFile.GetStream();
        }

        private async void uploadUserImage(Stream stream)
        {
            try
            {
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=doghaven2storage;" +
                                                    "AccountKey=Arp3X8RJ/LG4FMED/DqRMkWJn5Ba0IUhEdTJak6z5yOHcIsx+" +
                                                    "97bgwfjuQNBLgmpt+0J0mjK8rcCGeeMJ/FT0Q==;EndpointSuffix=core.w" +
                                                    "indows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("useraccountimagescontainear");

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
                App.doginfo_flag = 0;
                await DisplayAlert("Warning", "Something went wrong with uploading the image", "Okay");
                return;
            }
        }

        private void DeleteDog_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Navigation.PushAsync(new DeleteDogPage());
        }

        private void infoInitializer()
        {
            if (url != "")
            {
                saveInformation();
            }
        }
    }
}