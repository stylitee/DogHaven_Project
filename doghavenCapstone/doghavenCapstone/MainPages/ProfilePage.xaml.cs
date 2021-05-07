using doghavenCapstone.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            loadAccountInfo();
            profilePage.Add(this);
            BindingContext = this;
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
            App.doginfo_flag = 1;
            base.OnAppearing();
        }

        private async void loadAccountInfo()
        {
            var userInfo = await App.client.GetTable<accountusers>().Where(x => x.id == "e9f70cd415").ToListAsync();
            var addressInfo = await App.client.GetTable<usersaddress>().Where(x => x.id == userInfo[0].address_id).ToListAsync();
            var usertypeInfo = await App.client.GetTable<userRole>().Where(x => x.id == userInfo[0].user_role_id).ToListAsync();
            var dogInformation = await App.client.GetTable<dogInfo>().Where(x => x.userid == "e9f70cd415").ToListAsync();
            imgUser.Source = userInfo[0].userImage;
            lblName.Text = "Name: " + userInfo[0].fullName;
            lblAddress.Text = "Address: " + addressInfo[0].streetname + ", " + addressInfo[0].barangay;
            lblUserType.Text = usertypeInfo[0].roleDescription;
            lblDogsOwn.Text = "No. of dogs owned: " + dogInformation.Count.ToString();
            foreach(var info in dogInformation)
            {
                var dogbreed = await App.client.GetTable<dogBreed>().Where(x => x.id == info.dogBreed_id).ToListAsync();
                var purpose = await App.client.GetTable<dogPurpose>().Where(x => x.id == info.dogPurpose_id).ToListAsync();
                _Doglist.Add(new dogInfo()
                {
                    id = info.id,
                    dogPurpose_id = info.dogPurpose_id,
                    dogBreed_id = info.dogBreed_id,
                    userid = info.userid,
                    dogName = info.dogName,
                    dogGender = "Gender: " + info.dogGender,
                    dogImage = info.dogImage,
                    breed_Name = "Breed: " + dogbreed[0].breedName,
                    purposeDesc = "Purpose: " + purpose[0].dogDesc
                });
            }

        }
    }
}