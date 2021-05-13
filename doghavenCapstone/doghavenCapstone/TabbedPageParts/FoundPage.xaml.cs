using doghavenCapstone.ClassHelper;
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
    public partial class FoundPage : ContentPage
    {
        public ObservableCollection<FoundDogs> _FoundDogList = new ObservableCollection<FoundDogs>();
        public static List<ContentPage> FoundPageContent = new List<ContentPage>();
        string fullFoundAddress = "";
        public FoundPage()
        {
            InitializeComponent();
            FoundPageContent.Add(this);
            BindingContext = this;
        }

        public async void getAddress(double latitude, double longtitude)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longtitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                string geocodeAddress = placemark.SubThoroughfare + ", " + placemark.Thoroughfare + ", " + placemark.Locality + ", "
                                        + placemark.FeatureName + ", " + placemark.AdminArea + ", " + placemark.CountryName;
                fullFoundAddress = geocodeAddress;
            }
        }

        protected override void OnAppearing()
        {
            App.uploadFlag = 1;
            LoadFoundDogs();
            base.OnAppearing();

        }

        private async void LoadFoundDogs()
        {
            _FoundDogList.Clear();
            var LostList = await App.client.GetTable<FoundDogs>().ToListAsync();
            string breed_name = "", full_Name = "", dog_id = "", user__id = "", dogImage_source = "";
            foreach (var c in LostList)
            {
                getAddress(double.Parse(c.placeFound_latitude), double.Parse(c.placeFound_longtitude));
                var getDogInfo = await App.client.GetTable<dogInfo>().Where(x => x.id == c.dogInfo_id).ToListAsync();
                foreach (var info in getDogInfo)
                {
                    user__id = info.userid;
                    dog_id = info.dogBreed_id;
                    dogImage_source = info.dogImage;
                }
                var getBreedName = await App.client.GetTable<dogBreed>().Where(x => x.id == dog_id).ToListAsync();
                foreach (var result in getBreedName)
                {
                    breed_name = result.breedName;
                }
                var getUserInfo = await App.client.GetTable<accountusers>().Where(x => x.id == user__id).ToListAsync();
                foreach (var name in getUserInfo)
                {
                    full_Name = name.fullName;
                }
                _FoundDogList.Add(new FoundDogs()
                {
                    id = c.id,
                    dogImageSouce = dogImage_source,
                    userid = c.userid,
                    found_date = c.found_date,
                    found_time = c.found_time,
                    placeFound_latitude = c.placeFound_latitude,
                    placeFound_longtitude = c.placeFound_longtitude,
                    dogInfo_id = c.dogInfo_id,
                    fullName = "Owner: " + full_Name,
                    breedName = "Breed: " + breed_name,
                    dateLost = "Date Lost: " + c.found_date,
                    timeLost = "Time Lost: " + c.found_time,
                    placeLost = fullFoundAddress
                });
            }
        }

        public ObservableCollection<FoundDogs> FoundDogList
        {
            get => _FoundDogList;
            set
            {
                _FoundDogList = value;
            }
        }

        private void toolBarItemDog_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFoundDogPage());
        }

        private void btnSeeAllDogs_Clicked(object sender, EventArgs e)
        {
            VariableStorage.lostAndFoundIdentifier = "Found";
            Navigation.PushAsync(new AllLostDogsMap());
        }
    }
}