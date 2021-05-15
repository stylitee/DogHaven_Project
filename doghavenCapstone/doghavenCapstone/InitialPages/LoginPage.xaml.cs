using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
using doghavenCapstone.LocalDBModel;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.InitialPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        bool usercharacterChecker = false;
        bool passcharacterChecker = false;
        public static string user_Image = "", user_fullName = "";
        string _id = "", _fullName = "", _username = "", _password = "";
        public LoginPage()
        {
            InitializeComponent();
            lblErrorMessage.IsVisible = false;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var assembly = typeof(LoginPage);
            imgLogo.Source = ImageSource.FromResource("doghavenCapstone.Assets.Logo_icon.png", assembly);
        }

        public void errorMessage(string message)
        {
            lblErrorMessage.IsVisible = true;
            lblErrorMessage.Text = message;
        }

        

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //forgotPassword
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Please wait");
            Navigation.PushAsync(new Register());
        }

        private void txtUser_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtUser_name.Text.Length >= 4)
            {
                usercharacterChecker = true;
            }
            else
            {
                usercharacterChecker = false;
            }
        }

        private void txtUser_password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUser_password.Text.Length >= 4)
            {
                passcharacterChecker = true;
            }
            else
            {
                passcharacterChecker = false;
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if(usercharacterChecker != false & passcharacterChecker !=  false)
            {
                App.buttonName = "Proceed";
                string usernames = "", password = "";
                try
                {
                    if (txtUser_name.Text == "" || txtUser_name.Text == null || txtUser_password.Text == "" || txtUser_password.Text == null)
                    {
                        errorMessage("Fields cannot be empty");
                        txtUser_password.Text = "";
                        txtUser_name.Text = "";
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading("Logging in please wait");

                        var user = await App.client.GetTable<accountusers>().Where(u => u.username == txtUser_name.Text).ToListAsync();
                        foreach (var c in user)
                        {
                            App.fullName = c.fullName;
                            App.user_id = c.id;
                            usernames = c.username;
                            password = c.userPassword;

                            _id = c.id;
                            _fullName = c.fullName;
                            _username = c.username;
                            _password = c.userPassword;
                            user_Image = c.userImage;
                            user_fullName = c.fullName;
                        }

                        if (user != null)
                        {
                            if (password == txtUser_password.Text)
                            {
                                UserDialogs.Instance.ShowLoading("Please wait while we prepare everything for you");
                                accountsLoggedIn account = new accountsLoggedIn()
                                {
                                    userid = _id,
                                    fullName = _fullName,
                                    username = _username,
                                    userPassword = _password,
                                    isLoggedIn = "Yes"
                                };

                                SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation);
                                conn.CreateTable<accountsLoggedIn>();
                                conn.Insert(account);
                                conn.Close();

                                var newAccountChecker = await App.client.GetTable<dogInfo>().ToListAsync();
                                var dogchecker = await App.client.GetTable<dogInfo>().ToListAsync();
                                var userDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
                                var location = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == App.user_id).ToListAsync();
                                if (newAccountChecker.Count + 1 <= 5 && dogchecker.Count <= 5 && location.Count == 0)
                                {
                                    await Navigation.PushAsync(new GetUsersLocation());
                                }
                                if (newAccountChecker.Count + 1 <= 5 && dogchecker.Count <= 5 && location.Count != 0)
                                {
                                    if (userDogs.Count != 0)
                                    {
                                        await Navigation.PushAsync(new UploadDogPage());
                                    }
                                    else
                                    {
                                        await Navigation.PushAsync(new NewAccountVerify());
                                    }

                                }
                                if (newAccountChecker.Count() + 1 >= 5 && dogchecker.Count >= 5 && location.Count != 0)
                                {
                                    Application.Current.MainPage = new NavigationPage(new HomeFlyOut());
                                    txtUser_password.Text = "";
                                    txtUser_name.Text = "";
                                    usernames = "";
                                    password = "";
                                    UserDialogs.Instance.HideLoading();
                                }
                                if (newAccountChecker.Count() + 1 >= 5 && dogchecker.Count >= 5 && location.Count == 0)
                                {
                                    await Navigation.PushAsync(new GetUsersLocation());
                                }
                            }
                            else
                            {
                                txtUser_password.Text = "";
                                txtUser_name.Text = "";
                                UserDialogs.Instance.HideLoading();
                                errorMessage("The username or password you entered is incorrect");
                            }
                        }
                        else
                        {

                            txtUser_password.Text = "";
                            txtUser_name.Text = "";
                            UserDialogs.Instance.HideLoading();
                            errorMessage("The username or password you entered is incorrect");
                        }
                    }
                }
                catch(Microsoft.WindowsAzure.MobileServices.MobileServiceInvalidOperationException)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Sorry", "Something went wrong with the server, please try again in a few moments", "Okay");
                }
                catch(System.Net.Http.HttpRequestException)
                {
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new InternetChecker());
                }
                catch (Exception fs)
                {
                    await DisplayAlert("Warning", "Something went wrong:" + fs.Message, "Okay");
                    UserDialogs.Instance.HideLoading();
                }
            }
            else
            {
                errorMessage("Username or password cannot be less than 4");
            }
            
        }
    }
}