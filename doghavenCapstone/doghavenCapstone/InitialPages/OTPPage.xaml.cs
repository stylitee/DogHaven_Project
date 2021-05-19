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
    public partial class OTPPage : ContentPage
    {
        public OTPPage()
        {
            InitializeComponent();
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            if(Register.OTPResult.ToString() == txtOTP.Text)
            {
                await App.client.GetTable<usersaddress>().InsertAsync(Register._address[0]);
                await App.client.GetTable<accountusers>().InsertAsync(Register._accounts[0]);
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                Acr.UserDialogs.UserDialogs.Instance.Toast("Account successfully saved", new TimeSpan(2));
            }
            else
            {
                await DisplayAlert("Ops","You've enter the wrong one time password","Okay");
            }
        }
    }
}