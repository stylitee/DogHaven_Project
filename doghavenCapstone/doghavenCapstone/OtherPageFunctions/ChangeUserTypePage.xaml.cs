using doghavenCapstone.ClassHelper;
using doghavenCapstone.InitialPages;
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
    public partial class ChangeUserTypePage : ContentPage
    {
        public List<userRole> lstOfId = new List<userRole>();
        public ChangeUserTypePage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        public async void loadList()
        {
            var userTypes = await App.client.GetTable<userRole>().ToListAsync();
            foreach(var c in userTypes)
            {
                lstOfId.Add(c);
                pckrUserType.Items.Add(c.roleDescription);
            }
        }

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {
            updateProcess();
            //show a message that the updating is succesful
            Application.Current.MainPage = new NavigationPage( new NewAccountVerify());
            Navigation.PushAsync(new NewAccountVerify());
        }

        public async void updateProcess()
        {
            var account_details = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            var result = lstOfId.FindIndex(role => role.roleDescription ==
                                                          pckrUserType.Items[pckrUserType.SelectedIndex]);
            var userrole_id = lstOfId[result].id;
            string userType = pckrUserType.SelectedItem.ToString();
            foreach(var g in account_details)
            {
                accountusers user = new accountusers()
                {
                    id = App.user_id,
                    username = g.username,
                    userImage = g.userImage,
                    userPassword = g.userPassword,
                    fullName = g.fullName,
                    address_id = g.address_id,
                    user_role_id = userrole_id
                };

                accountusers.Update(user);
            }
            
            lstOfId.Clear();
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewAccountVerify());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadList();
        }
    }
}