using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ObservableCollection<dogInfo> _Doglist = new ObservableCollection<dogInfo>();
        public static List<ContentPage> profilePage = new List<ContentPage>();
        public ProfilePage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            profilePage.Clear();
            profilePage.Add(this);
            BindingContext = this;
            App.doginfo_flag = 1;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        public ObservableCollection<dogInfo> DogList
        {
            get => _Doglist;
            set
            {
                _Doglist = value;
            }
        }

        protected override void OnAppearing()
        {
            App._updateflag = true;
            loadAccountInfo();
            App.uploadFlag = 1;
            App.doginfo_flag = 0;
            base.OnAppearing();
        }

        private async void loadAccountInfo()
        {
            var userInfo = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            var addressInfo = await App.client.GetTable<usersaddress>().Where(x => x.id == userInfo[0].address_id).ToListAsync();
            var usertypeInfo = await App.client.GetTable<userRole>().Where(x => x.id == userInfo[0].user_role_id).ToListAsync();
            var dogInformation = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
            imgUser.Source = userInfo[0].userImage;
            lblName.Text = "Name: " + userInfo[0].fullName;
            lblAddress.Text = "Address: " + addressInfo[0].streetname + ", " + addressInfo[0].barangay;
            lblUserType.Text = usertypeInfo[0].roleDescription;
            lblDogsOwn.Text = "No. of dogs owned: " + dogInformation.Count.ToString();
            _Doglist.Clear();
            foreach (var info in dogInformation)
            {
                var dogbreed = await App.client.GetTable<dogBreed>().Where(x => x.id == info.dogBreed_id).ToListAsync();
                var purpose = await App.client.GetTable<dogPurpose>().Where(x => x.id == info.dogPurpose_id).ToListAsync();
                _Doglist.Add(new dogInfo()
                {
                    id = info.id,
                    /*dogPurpose_id = info.dogPurpose_id,*/
                    /*dogBreed_id = info.dogBreed_id,*/
                    userid = info.userid,
                    dogName = info.dogName,
                    dogGender = "Gender: " + info.dogGender,
                    dogImage = info.dogImage,
                    dogBreed_id = "Breed: " + dogbreed[0].breedName,
                    dogPurpose_id = "Purpose: " + purpose[0].dogDesc
                });
            }

        }

        private void btnLogOut_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LogoutPage());
        }

        private void btnUpdate_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateAccount());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePassword());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SellerTypeApplication());
        }
    }
}