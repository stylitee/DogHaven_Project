using doghavenCapstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.PreventerPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindAccountPage : ContentPage
    {
        public static int OTPResult = 0;
        public static string username = "";
        public FindAccountPage()
        {
            InitializeComponent();
        }

        private async void txtFindAccount_Clicked(object sender, EventArgs e)
        {
            if(txtFindAccount.Text == "" || txtFindAccount.Text.Length < 3)
            {
                await DisplayAlert("Ops","Please enter a valid username","Okay");
            }
            else
            {
                var result = await App.client.GetTable<accountusers>().Where(x => x.username == txtUsername.Text).ToListAsync();
                if(result.Count != 0)
                {
                    OTP();
                    username = txtUsername.Text;
                    await DisplayAlert("Confirmation", "A code has been sent to your phone number that is registered with this account", "Okay");
                }
                else
                {
                    await DisplayAlert("Sorry", "We couldn't find your account", "Okay");
                }
            }
        }

        private async void OTP()
        {
            try
            {
                var number = await App.client.GetTable<accountusers>().Where(x => x.username == txtUsername.Text).ToListAsync();
                string accountSid = "AC31df88e462474edfa661c409f0c0806a";
                string authToken = "b088a7e257a4dc3fc533fd39c6fde692";
                OTPResult = GenerateOTP();
                string final_num = "+63" + number[0].phoneNumber.Remove(0, 1);

                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    body: "You're recover code for DogHaven Account is " + OTPResult.ToString(),
                    from: new Twilio.Types.PhoneNumber("+15634122473"),
                    to: new Twilio.Types.PhoneNumber(final_num)
                );

                await Navigation.PushAsync(new ForgotPasswordOTPPage());
            }
            catch (System.Net.Http.HttpRequestException)
            {
                await Navigation.PushAsync(new InternetChecker());
            }
        }

        public int GenerateOTP()
        {
            return new Random().Next(1000, 9999);
        }
    }
}