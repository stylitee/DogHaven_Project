using Acr.UserDialogs;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
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
        //dont forget to make user-id null or empty in logoutpage
        public LoginPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var assembly = typeof(LoginPage);

            imgLogo.Source = ImageSource.FromResource("doghavenCapstone.Assets.Logo_icon.png", assembly);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess.ToString() != "Internet" && App.connectionFlag == 0)
            {
                Navigation.PushAsync(new InternetChecker());
                App.connectionFlag = 1;
            }
            else
            {
                Navigation.PopAsync();
                App.connectionFlag = 0;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //login
            App.buttonName = "Proceed";
            string usernames = "", password = "";
            try
            {
                if (txtUser_name.Text == "" || txtUser_name.Text == null || txtUser_password.Text == "" || txtUser_password.Text == null)
                {
                    await DisplayAlert("Warning", "Fields cannot be empty", "Okay");
                    txtUser_password.Text = "";
                    txtUser_name.Text = "";
                }
                else
                {
                    //App.loadingMessage = "Logging in please wait . . .";
                    //await Navigation.PushAsync(new Loading());
                    UserDialogs.Instance.ShowLoading("Logging in please wait");
                    
                    var user = await App.client.GetTable<accountusers>().Where(u => u.username == txtUser_name.Text).ToListAsync();
                    foreach (var c in user)
                    {
                        App.fullName = c.fullName;
                        App.user_id = c.id;
                        usernames = c.username;
                        password = c.userPassword;
                    }

                    if (user != null)
                    {
                        if (password == txtUser_password.Text)
                        {
                            UserDialogs.Instance.ShowLoading("Please wait while we prepare everything for you");
                            var newAccountChecker = await App.client.GetTable<dogInfo>().ToListAsync();
                            var dogchecker = await App.client.GetTable<dogInfo>().ToListAsync();
                            var userDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
                            var location = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == App.user_id).ToListAsync();
                            if (newAccountChecker.Count + 1 <= 5 && dogchecker.Count <= 5 && location.Count == 0)
                            {
                                await Navigation.PushAsync(new GetUsersLocation());
                            }
                            if(newAccountChecker.Count + 1 <= 5 && dogchecker.Count <= 5 && location.Count != 0)
                            {
                                //await Navigation.PushAsync(new UploadDogPage());
                                if (userDogs.Count != 0)
                                {
                                    await Navigation.PushAsync(new UploadDogPage());
                                }
                                else
                                {
                                    await Navigation.PushAsync(new NewAccountVerify());
                                }

                            }
                            if(newAccountChecker.Count() + 1 >= 5 && dogchecker.Count >= 5 && location.Count != 0)
                            {
                                Application.Current.MainPage = new HomeFlyOut();
                                await Navigation.PushAsync(new HomeFlyOut());
                                txtUser_password.Text = "";
                                txtUser_name.Text = "";
                                usernames = "";
                                password = "";
                                UserDialogs.Instance.HideLoading();
                            }
                            if(newAccountChecker.Count() + 1 >= 5 && dogchecker.Count >= 5 && location.Count == 0)
                            {
                                await Navigation.PushAsync(new GetUsersLocation());
                            }
                        }
                        else
                        {
                            txtUser_password.Text = "";
                            txtUser_name.Text = "";
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert("Warning", "Invalid username or password", "Okay");
                        }
                    }
                    else
                    {
                        
                        txtUser_password.Text = "";
                        txtUser_name.Text = "";
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Warning", "Invalid username or password", "Okay");
                    }
                }
            }
            catch (Exception fs)
            {
                await DisplayAlert("Warning", "Something went wrong:" + fs.Message , "Okay");
                UserDialogs.Instance.HideLoading();
                throw;
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //forgotPassword
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            //register now
            Navigation.PushAsync(new Register());
        }
    }
}