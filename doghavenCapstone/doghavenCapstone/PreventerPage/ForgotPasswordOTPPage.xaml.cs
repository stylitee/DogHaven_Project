using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.PreventerPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordOTPPage : ContentPage
    {
        public ForgotPasswordOTPPage()
        {
            InitializeComponent();
        }

        private void btnConfirm_Clicked_1(object sender, EventArgs e)
        {
            if(FindAccountPage.OTPResult.ToString() == txtCode.Text)
            {
                Navigation.PushAsync(new RecoverPasswordPage());
            }
            else
            {
                DisplayAlert("Ops","Youve enter a wrong code","Okay");
            }
        }
    }
}