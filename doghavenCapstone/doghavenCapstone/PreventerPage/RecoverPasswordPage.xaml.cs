using doghavenCapstone.Model;
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
    public partial class RecoverPasswordPage : ContentPage
    {
        public RecoverPasswordPage()
        {
            InitializeComponent();
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            if(txtPassword.Text == "" || txtConfirmPassword.Text == "")
            {
                await DisplayAlert("Ops", "Fields cannot be empty", "Okay");
            }
            else
            {
                if(txtPassword.Text != txtConfirmPassword.Text)
                {
                    await DisplayAlert("Ops", "Your password doesn't match", "Okay");
                }
                else
                {
                    var info = await App.client.GetTable<accountusers>().Where(x => x.username == FindAccountPage.username).ToListAsync();
                    accountusers accountusers = new accountusers()
                    {
                        id = info[0].id,
                        userImage = info[0].userImage,
                        username = info[0].username,
                        userPassword = txtConfirmPassword.Text,
                        fullName = info[0].fullName,
                        address_id = info[0].address_id,
                        user_role_id = info[0].user_role_id,
                        phoneNumber = info[0].phoneNumber
                    };

                    await App.client.GetTable<accountusers>().UpdateAsync(accountusers);
                    await DisplayAlert("Confirmation","You're account has been recovered succesfully","Okay");
                }
            }
        }
    }
}