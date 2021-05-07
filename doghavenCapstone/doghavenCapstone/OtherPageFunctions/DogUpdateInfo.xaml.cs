using Acr.UserDialogs;
using doghavenCapstone.Model;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogUpdateInfo : ContentPage
    {
        List<dogBreed> lstofBreeds = new List<dogBreed>();
        List<dogPurpose> lstofPurpose = new List<dogPurpose>();
        string defaultImage = "";
        Stream dog_image = null;
        string url = "";
        public DogUpdateInfo()
        {
            InitializeComponent();
            loadPickers();
            loadInformation();
        }

        private async void loadPickers()
        {
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
        }

        private async void loadInformation()
        {
            var doginfo = await App.client.GetTable<dogInfo>().Where(x => x.id == App.dog_id).ToListAsync();
            var breedinfo = await App.client.GetTable<dogInfo>().Where(x => x.id == doginfo[0].dogBreed_id).ToListAsync();
            var purposeinfo = await App.client.GetTable<dogInfo>().Where(x => x.id == doginfo[0].dogPurpose_id).ToListAsync();
            txtName.Text = doginfo[0].dogName;
            pckrGender.SelectedItem = doginfo[0].dogGender;
            pckrBreed.SelectedItem = breedinfo[0].breed_Name;
            pckrPurpose.SelectedItem = purposeinfo[0].purposeDesc;
            imgDog.Source = doginfo[0].dogImage;
            defaultImage = doginfo[0].dogImage;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void btnUpdate_Clicked(object sender, EventArgs e)
        {
            uploadUserImage(dog_image);
        }

        public void saveInformation()
        {
            try
            {
                if(txtName.Text != "")
                {
                    dogInfo info = new dogInfo();
                    info.id = App.dog_id;
                    info.dogName = txtName.Text;
                    if (url != "")
                    {
                        info.dogImage = url;
                    }

                    if (url == "")
                    {
                        info.dogImage = defaultImage;
                    }
                    info.dogGender = pckrGender.Items[pckrGender.SelectedIndex].ToString();
                    var index = lstofBreeds.FindIndex(breed => breed.breedName ==
                                                              pckrBreed.Items[pckrBreed.SelectedIndex]);
                    var breed_id = lstofBreeds[index].id;
                    info.dogBreed_id = breed_id;
                    var i = lstofPurpose.FindIndex(purpose => purpose.dogDesc ==
                                                              pckrPurpose.Items[pckrPurpose.SelectedIndex]);
                    var purpose_id = lstofBreeds[i].id;
                    info.dogPurpose_id = purpose_id;

                    dogInfo.Update(info);
                    DisplayAlert("Confirmation", "Dog is succesfully updated", "Okay");
                }
                else
                {
                    DisplayAlert("Ops!", "Some fields cannot be empty", "Okay");
                }

            }
            catch (Exception)
            {

                throw;
            }
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
                await DisplayAlert("Warning", "Something went wrong with uploading the image", "Okay");
                return;
            }
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