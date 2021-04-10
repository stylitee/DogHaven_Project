using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var assembly = typeof(LoginPage);

            imgLogo.Source = ImageSource.FromResource("doghavenCapstone.Assets.Logo_icon.png", assembly);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //login
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
                    App.loadingMessage = "Logging in please wait . . .";
                    await Navigation.PushAsync(new Loading());
                    var user = await App.client.GetTable<accountusers>().Where(u => u.username == txtUser_name.Text).ToListAsync();
                    foreach (var c in user)
                    {
                        App.user_id = c.id;
                        usernames = c.username;
                        password = c.userPassword;
                    }

                    if (user != null)
                    {
                        if (password == txtUser_password.Text)
                        {
                            Application.Current.MainPage = new HomeFlyOut();
                            await Navigation.PushAsync(new HomeFlyOut());
                            txtUser_password.Text = "";
                            txtUser_name.Text = "";
                            usernames = "";
                            password = "";
                            App.loadingMessage = "";
                        }
                        else
                        {
                            await Navigation.PushAsync(new LoginPage());
                            App.loadingMessage = "";
                            txtUser_password.Text = "";
                            txtUser_name.Text = "";
                            await DisplayAlert("Warning", "Invalid username or password", "Okay");
                        }
                    }
                    else
                    {
                        await Navigation.PushAsync(new LoginPage());
                        App.loadingMessage = "";
                        txtUser_password.Text = "";
                        txtUser_name.Text = "";
                        await DisplayAlert("Warning", "Invalid username or password", "Okay");
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Warning", "Something went wrong", "Okay");
                App.loadingMessage = "";
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