using Acr.UserDialogs;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
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

namespace doghavenCapstone.InitialPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        Stream dog_image = null;
        string url = "";
        List<userRole> lstofRoles = new List<userRole>();
        public Register()
        {
            InitializeComponent();

            var assembly = typeof(Register);

            //imgLogo.Source = ImageSource.FromResource("doghavenCapstone.Assets.Logo_icon.png", assembly);
            loadPicker();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //-------------------register button------------------------
            uploadUserImage(dog_image);
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //login
            Navigation.PushAsync(new LoginPage());
        }

        private async void imgUsersImage_Tapped(object sender, EventArgs e)
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

            imgUsersImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            
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
            if(url != "")
            {
                savingInformation();
            }
        }

        //UserDialogs.Instance.ShowLoading("Please wait while we save your account");
        private async void savingInformation()
        {
            string addressid = "", userrole_id = "";

            if (txtUser_name.Text == "" || txtUser_name.Text == null ||
                txtConfirmPassword.Text == "" || txtConfirmPassword.Text == null ||
                txtFullname.Text == "" ||
                txtStreetName.Text == "" || txtStreetName.Text == null ||
                txtBarangay.Text == "" || txtBarangay.Text == null ||
                pckrUserRole == null || imgUsersImage == null)
            {
                await DisplayAlert("Warning", "Fields and Images cannot be empty", "Okay");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {

                    try
                    {
                        UserDialogs.Instance.ShowLoading("Information is being processed, please wait");
                        addressid = System.Guid.NewGuid().ToString("N").Substring(0, 11);

                        usersaddress address = new usersaddress()
                        {
                            id = addressid,
                            streetname = txtStreetName.Text,
                            barangay = txtBarangay.Text
                        };

                        var result = lstofRoles.FindIndex(role => role.roleDescription ==
                                                          pckrUserRole.Items[pckrUserRole.SelectedIndex]);
                        userrole_id = lstofRoles[result].id;

                        accountusers user = new accountusers()
                        {
                            id = Guid.NewGuid().ToString("N").Substring(0, 10),
                            userImage = url,
                            username = txtUser_name.Text,
                            userPassword = txtConfirmPassword.Text,
                            fullName = txtFullname.Text,
                            address_id = addressid,
                            user_role_id = userrole_id
                        };

                        await App.client.GetTable<usersaddress>().InsertAsync(address);
                        await App.client.GetTable<accountusers>().InsertAsync(user);
                        await Navigation.PushAsync(new LoginPage());
                        UserDialogs.Instance.HideLoading();
                        Acr.UserDialogs.UserDialogs.Instance.Toast("Account successfully saved", new TimeSpan(2));
                        //await DisplayAlert("Confirmation", "You're acccount is succesfully saved!", "Okay");
                        //App.loadingMessage = "";
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Error", "Connection error, please try again later", "Okay");
                    }
                }
                else
                {
                    await DisplayAlert("Ops", "Password does not match", "Okay");
                }

            }
            addressid = "";
            userrole_id = "";
            clearFields();
            loadPicker();
        }
        public async void loadPicker()
        {
            var userAddressTable = await App.client.GetTable<userRole>().ToListAsync();
            foreach (var role in userAddressTable)
            {
                lstofRoles.Add(role);
                pckrUserRole.Items.Add(role.roleDescription);
            }
        }

        public void clearFields()
        {
            
            txtUser_name.Text = "";
            txtFullname.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtBarangay.Text = "";
            txtStreetName.Text = "";
            pckrUserRole.Items.Clear();
        }
    }
}