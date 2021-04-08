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
        List<userRole> lstofRoles = new List<userRole>();
        public Register()
        {
            InitializeComponent();

            var assembly = typeof(LoginPage);

            //imgLogo.Source = ImageSource.FromResource("doghavenCapstone.Assets.Logo_icon.png", assembly);
            loadPicker();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //-------------------register button------------------------
            string addressid = "", userrole_id = "";

            if (txtUser_name.Text == "" || txtUser_name.Text == null || 
                txtConfirmPassword.Text == "" || txtConfirmPassword.Text == null ||
                txtfname.Text == "" || txtfname.Text == null ||
                txtmname.Text == "" || txtmname.Text == null ||
                txtlname.Text == "" || txtlname.Text == null ||
                txtStreetName.Text == "" || txtStreetName.Text == null ||
                txtBarangay.Text == "" || txtBarangay.Text == null ||
                pckrUserRole == null)
            {
                await DisplayAlert("Warning", "Fields cannot be empty", "Okay");
            }
            else
            {
                if(txtPassword.Text == txtConfirmPassword.Text)
                {
                    
                    try
                    {
                        addressid = System.Guid.NewGuid().ToString("N").Substring(0, 11);
                        
                        usersaddress address = new usersaddress()
                        {
                            id = addressid,
                            streetname = txtStreetName.Text,
                            barangay = txtBarangay.Text
                        };

                        var result = lstofRoles.FindIndex(role => role.roleDescription == 
                                                          pckrUserRole.Items[pckrUserRole.SelectedIndex]);
                        userrole_id = lstofRoles[result].id;

                        accountusers user = new accountusers()
                        {
                            id = Id.ToString("N").Substring(0, 10),
                            username = txtUser_name.Text,
                            userPassword = txtConfirmPassword.Text,
                            firstName = txtfname.Text,
                            middleName = txtmname.Text,
                            lastName = txtlname.Text,
                            address_id = addressid,
                            user_role_id = userrole_id
                        };

                        await App.client.GetTable<usersaddress>().InsertAsync(address);
                        await App.client.GetTable<accountusers>().InsertAsync(user);
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Error", "Connection error, please try again later", "Okay");
                    }
                }
                else
                {
                    await DisplayAlert("Ops", "Password does not match", "Okay");
                }
                
            }
            addressid = "";
            userrole_id = "";
            clearFields();
            loadPicker();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //login
            Navigation.PushAsync(new LoginPage());
        }

        public async void loadPicker()
        {
            var userAddressTable = await App.client.GetTable<userRole>().ToListAsync();
            foreach (var role in userAddressTable)
            {
                lstofRoles.Add(role);
                pckrUserRole.Items.Add(role.roleDescription);
            }
        }

        public void clearFields()
        {
            txtUser_name.Text = "";
            txtfname.Text = "";
            txtmname.Text = "";
            txtlname.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtBarangay.Text = "";
            txtStreetName.Text = "";
            pckrUserRole.Items.Clear();
        }
    }
}