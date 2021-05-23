using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePassword : ContentPage
	{
		public ChangePassword ()
		{
			InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
		}

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnConfirm_Clicked_1(object sender, EventArgs e)
        {
            var userInformation = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            var passResult = AppHelpers.PasswordDecrypt(userInformation[0].userPassword);
            if (txtOldPassword.Text != passResult)
            {
                await DisplayAlert("Ops", "Old password is incorrect", "okay");
            }
            else
            {
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    await DisplayAlert("Ops", "Your password doesnt match", "okay");
                }
                else
                {
                    accountusers user = new accountusers()
                    {
                        id = userInformation[0].id,
                        userImage = userInformation[0].userImage,
                        username = userInformation[0].username,
                        userPassword = AppHelpers.PasswordEncryption(txtConfirmPassword.Text),
                        fullName = userInformation[0].fullName,
                        address_id = userInformation[0].address_id,
                        user_role_id = userInformation[0].user_role_id,
                        phoneNumber = userInformation[0].phoneNumber
                    };

                    accountusers.Update(user);
                    await DisplayAlert("Confirmation", "Password succesfully changed", "Okay");
                }
            }
        }
    }
}