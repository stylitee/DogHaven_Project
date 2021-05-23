using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.InitialPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        Stream dog_image = null;
        bool _usernameChecker = false;
        bool _pass = false, _confirmpass = false;
        string url = "";
        public static int OTPResult = 0;
        public static List<accountusers> _accounts = new List<accountusers>();
        public static List<usersaddress> _address = new List<usersaddress>();
        List<userRole> lstofRoles = new List<userRole>();
        public Register()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var assembly = typeof(Register);
            lblIndicator.BackgroundColor = Color.Yellow;
            imgUsersImage.Source = ImageSource.FromResource("doghavenCapstone.Assets.no_image_available.jpg", assembly);
            UserDialogs.Instance.HideLoading();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //-------------------register button------------------------
            if (txtUser_name.Text == "" || txtUser_name.Text == null ||
                txtConfirmPassword.Text == "" || txtConfirmPassword.Text == null ||
                txtFullname.Text == "" ||
                txtStreetName.Text == "" || txtStreetName.Text == null ||
                txtBarangay.Text == "" || txtBarangay.Text == null ||
                imgUsersImage == null || txtCity.Text == "" || txtProvince.Text == "")
            {
                await DisplayAlert("Warning", "Fields and Images cannot be empty", "Okay");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                if (_usernameChecker != false)
                {
                    UserDialogs.Instance.ShowLoading("Please wait while we register your account");
                    uploadUserImage(dog_image);
                }
                else
                {
                    await DisplayAlert("Ops", "Username is already use please use another one", "Okay");
                    UserDialogs.Instance.HideLoading();
                }
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //login
            Navigation.PushAsync(new LoginPage());
        }

        private async void imgUsersImage_Tapped(object sender, EventArgs e)
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

                imgUsersImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                dog_image = selectedImageFile.GetStream();
            }
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {
                await DisplayAlert("Permission Error", "We need your permission to access your gallery", "Okay");
            }
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
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        private async void savingInformation()
        {
            _accounts.Clear();
            _address.Clear();
            if (_pass != false && _confirmpass != false)
            {
                string addressid = "", userrole_id = "";

                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    if(txtPhoneNumber.Text.Substring(0,2) == "09")
                    {
                        try
                        {
                            UserDialogs.Instance.ShowLoading("Information is being processed, please wait");
                            addressid = System.Guid.NewGuid().ToString("N").Substring(0, 11);
                            string encryptedPass = AppHelpers.PasswordEncryption(txtConfirmPassword.Text);
                            usersaddress address = new usersaddress()
                            {
                                id = addressid,
                                streetname = txtStreetName.Text,
                                barangay = txtBarangay.Text,
                                city = txtCity.Text,
                                province = txtProvince.Text
                            };

                            accountusers user = new accountusers()
                            {
                                id = Guid.NewGuid().ToString("N").Substring(0, 10),
                                userImage = url,
                                username = txtUser_name.Text,
                                userPassword = encryptedPass,
                                fullName = txtFullname.Text,
                                address_id = addressid,
                                user_role_id = "abscenjs1",
                                phoneNumber = txtPhoneNumber.Text
                            };

                            _accounts.Add(user);
                            _address.Add(address);

                            OTP();
                            UserDialogs.Instance.HideLoading();
                            
                        }
                        catch (Exception)
                        {
                            await DisplayAlert("Error", "Connection error, please try again later", "Okay");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ops", "You've enter an invalid phone number", "Okay");
                    }
                    
                }
                else
                {
                    await DisplayAlert("Ops", "Password does not match", "Okay");
                }
                addressid = "";
                userrole_id = "";
                clearFields();
            }
            else
            {
                await DisplayAlert("Ops", "Password Cannot be less than four characters", "Okay");
            }
            
            UserDialogs.Instance.HideLoading();
        }

        public void clearFields()
        {
            
            txtUser_name.Text = "";
            txtFullname.Text = "";
            txtBarangay.Text = "";
            txtProvince.Text = "";
            txtCity.Text = "";
            txtStreetName.Text = "";
        }

        private void txtConfirmPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtConfirmPassword.Text.Length >= 4)
            {
                _confirmpass = true;
            }
            else
            {
                _confirmpass = false;
            }
        }

        private async void txtUser_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                lblIndicator.BackgroundColor = Color.Yellow;
                if (txtUser_name.Text.Length > 3)
                {
                    var usernameChecker = await App.client.GetTable<accountusers>().Where(x => x.username == txtUser_name.Text).ToListAsync();
                    if (usernameChecker.Count != 0)
                    {
                        lblIndicator.BackgroundColor = Color.Red;
                        _usernameChecker = false;
                    }
                    else
                    {
                        lblIndicator.BackgroundColor = Color.Green;
                        _usernameChecker = true;
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                await Navigation.PushAsync(new InternetChecker());
            }
        }

        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string val = txtPhoneNumber.Text;

            if (val.Length > 11)
            {
                val = val.Remove(val.Length - 1);
                txtPhoneNumber.Text = val;
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text.Length >= 4)
            {
                _pass = true;
            }
            else
            {
                _pass = false;
            }
        }

        private async void OTP()
        {
            try
            {
                string accountSid = "ACf7bf998164d8831baae2a9e2e2f5a64f";
                string authToken = "31c30e40bd0988e50f3f0bb398517843";
                OTPResult = GenerateOTP();
                string final_num = "+63" + txtPhoneNumber.Text.Remove(0, 1);

                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    body: "You're OTP for DogHaven Account is " + OTPResult.ToString(),
                    from: new Twilio.Types.PhoneNumber("+14079179741"),
                    to: new Twilio.Types.PhoneNumber(final_num)
                );

                await DisplayAlert("Confirmation", "A code has been sent to your phone number " + txtPhoneNumber.Text, "Okay");
                Application.Current.MainPage = new NavigationPage(new OTPPage());
            }
            catch (Twilio.Exceptions.ApiException)
            {
                await App.client.GetTable<usersaddress>().InsertAsync(_address[0]);
                await App.client.GetTable<accountusers>().InsertAsync(_accounts[0]);
                UserDialogs.Instance.Toast("Account succesfully saved", new TimeSpan(3));
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        public int GenerateOTP()
        {
            return new Random().Next(1000, 9999);
        }

    }
}