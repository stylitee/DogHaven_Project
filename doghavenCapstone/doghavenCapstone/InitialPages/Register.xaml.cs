using doghavenCapstone.Model;
using Newtonsoft.Json;
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
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        private async void btnSubmission_Clicked(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtUsername.Text == null || txtPassword.Text == "" || txtPassword.Text == null)
            {
                await DisplayAlert("Warning", "Fields cannot be empty", "Okay");
            }
            else
            {
                try
                {
                    accountusers user = new accountusers()
                    {
                        id = Id.ToString("N").Substring(0, 10),
                        username = txtUsername.Text,
                        userPassword = txtPassword.Text
                    };

                    await App.client.GetTable<accountusers>().InsertAsync(user);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}