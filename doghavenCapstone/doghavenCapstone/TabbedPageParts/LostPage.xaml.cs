using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.TabbedPageParts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LostPage : ContentPage
    {
        public ObservableCollection<LostDogs> _LostDoglist = new ObservableCollection<LostDogs>();
        public static List<ContentPage> LostPageContent = new List<ContentPage>();
        string fullLostAddress = "";
        public LostPage()
        {
            InitializeComponent();
            LostPageContent.Add(this);
            BindingContext = this;
            LoadLostDogs();
        }

        public async void getAddress(double latitude, double longtitude)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longtitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                string geocodeAddress = placemark.SubThoroughfare + ", " + placemark.Thoroughfare + ", " + placemark.Locality + ", "
                                        + placemark.FeatureName + ", " + placemark.AdminArea + ", " + placemark.CountryName;
                fullLostAddress = geocodeAddress;
            }
        }

        public async void LoadLostDogs()
        {
            var LostList = await App.client.GetTable<LostDogs>().ToListAsync();
            string breed_name = "", full_Name = "", dog_id = "", user__id ="",  dogImage_source = "";
            foreach(var c in LostList)
            {
                getAddress(double.Parse(c.placeLost_latitude),double.Parse(c.placeLost_longtitude));
                var getDogInfo = await App.client.GetTable<dogInfo>().Where(x => x.id == c.dogInfo_id).ToListAsync();
                foreach(var info in getDogInfo)
                {
                    user__id = info.userid;
                    dog_id = info.dogBreed_id;
                    dogImage_source = info.dogImage;
                }
                var getBreedName = await App.client.GetTable<dogBreed>().Where(x => x.id == dog_id).ToListAsync();
                foreach(var result in getBreedName)
                {
                    breed_name = result.breedName;
                }
                var getUserInfo = await App.client.GetTable<accountusers>().Where(x => x.id == user__id).ToListAsync();
                foreach(var name in getUserInfo)
                {
                    full_Name = name.fullName;
                }
                _LostDoglist.Add(new LostDogs()
                {
                    id = c.id,
                    dogImageSouce = dogImage_source,
                    userid = c.userid,
                    lastSeen_date = c.lastSeen_date,
                    lastSeen_time = c.lastSeen_time,
                    placeLost_longtitude = c.placeLost_longtitude,
                    placeLost_latitude = c.placeLost_latitude,
                    dogInfo_id = c.dogInfo_id,
                    fullName = "Owner: " + full_Name,
                    breedName = "Breed: " + breed_name,
                    dateLost = "Date Lost: " + c.lastSeen_date,
                    timeLost = "Time Lost: " + c.lastSeen_time,
                    placeLost = fullLostAddress
                });
            }
        }

        private void toolBarItemDog_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UploadDogPage());
        }

        public ObservableCollection<LostDogs> LostDoglist
        {
            get => _LostDoglist;
            set
            {
                _LostDoglist = value;
            }
        }
    }
}