using doghavenCapstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePassword : ContentPage
	{
		public ChangePassword ()
		{
			InitializeComponent ();
		}

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {

        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {

        }

        private async void updatePassword(Entry oldPassword, Entry newPassword, Entry confirmPassword)
        {
            var userInformation = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            if(oldPassword.Text != userInformation[0].userPassword)
            {
                await DisplayAlert("Ops", "Old password is incorrect", "okay");
            }
            else
            {
                if(newPassword.Text != confirmPassword.Text)
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
                        userPassword = confirmPassword.Text,
                        fullName = userInformation[0].fullName,
                        address_id = userInformation[0].address_id,
                        user_role_id = userInformation[0].user_role_id
                    };

                    accountusers.Update(user);
                    await DisplayAlert("Confirmation", "Password succesfully changed", "Okay");
                }
            }
        }
    }
}