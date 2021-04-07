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
        }

        private void btnRegister_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Register());
        }

        private async void btnSubmitted_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtUser.Text == "" || txtUser.Text == null || txtPword.Text == "" || txtPword.Text == null)
                {
                    await DisplayAlert("Warning", "Fields cannot be empty", "Okay");
                    txtPword.Text = "";
                    txtUser.Text = "";
                }
                else
                {
                    var user = await App.client.GetTable<accountusers>().Where(u => u.username == txtUser.Text).ToListAsync();

                    if (user != null)
                    {
                        await DisplayAlert("CONFIRMATION", "MAY LAMAN", "Okay");
                        /*if (user.userPassword == txtPword.Text)
                        {
                            await DisplayAlert("Confirmation", "Login Succesful", "Okay");
                            txtPword.Text = "";
                            txtUser.Text = "";
                        }*/
                    }
                    else
                    {
                        txtPword.Text = "";
                        txtUser.Text = "";
                        await DisplayAlert("Warning", "Invalid username or password", "Okay");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}