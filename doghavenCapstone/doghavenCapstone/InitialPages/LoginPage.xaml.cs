using doghavenCapstone.Model;
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
                    var user = await App.client.GetTable<accountusers>().Where(u => u.username == txtUser_name.Text).ToListAsync();

                    foreach (var c in user)
                    {
                        usernames = c.username;
                        password = c.userPassword;
                    }

                    if (user != null)
                    {
                        if (password == txtUser_password.Text)
                        {
                            await DisplayAlert("Confirmation", "Login Succesful", "Okay");
                            txtUser_password.Text = "";
                            txtUser_name.Text = "";
                            usernames = "";
                            password = "";
                        }
                    }
                    else
                    {
                        txtUser_password.Text = "";
                        txtUser_name.Text = "";
                        await DisplayAlert("Warning", "Invalid username or password", "Okay");
                    }
                }
            }
            catch (Exception)
            {

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