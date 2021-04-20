using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
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
    public partial class LostDogDetails : ContentPage
    {
        public LostDogDetails()
        {
            InitializeComponent();
            loadDetails();
            //getAddress();
        }

        public async void loadDetails()
        {
            string ownerName = "", date = VariableStorage.lastSeen_date, time = VariableStorage.lastSeen_time, 
                lastSeenAddress = "", breedID = "" ,BreedName = "", image = "";
            var ownerInfo = await App.client.GetTable<accountusers>().Where(x => x.id == VariableStorage.userid).ToListAsync();
            foreach(var details in ownerInfo)
            {
                ownerName = details.fullName;
            }

            var dogDetails = await App.client.GetTable<dogInfo>().Where(x => x.id == VariableStorage.doginfo_id).ToListAsync();
            foreach(var details in dogDetails)
            {
                image = details.dogImage;
                breedID = details.dogBreed_id;
            }
            var breedDetails = await App.client.GetTable<dogBreed>().Where(x => x.id == breedID).ToListAsync();
            foreach(var detail in breedDetails)
            {
                BreedName = detail.breedName;
            }

            var placemarks = await Geocoding.GetPlacemarksAsync(double.Parse(VariableStorage.placeLost_latitude), double.Parse(VariableStorage.placeLost_longtitude));
            var placemark = placemarks?.FirstOrDefault();

            var geocodeAddress = placemark.SubThoroughfare + ", " +
                                 placemark.Thoroughfare + ", " +
                                 placemark.Locality + ", " +
                                 placemark.SubAdminArea + ", " +
                                 placemark.AdminArea + "," +
                                 placemark.CountryName;

            lastSeenAddress = geocodeAddress;

            imgDogImage.Source = image;
            lblOwnerName.Text = "Owner name: " + ownerName;
            lblDate.Text = "Date Lost: " + VariableStorage.lastSeen_date;
            lblTime.Text = "Time Lost: " + VariableStorage.lastSeen_time;
            lblAddress.Text = "Lost in: " + lastSeenAddress;
            lblBreedName.Text = "Breed name: " + BreedName;
        }

        protected override void OnAppearing()
        {
            App.uploadFlag = 0;
            base.OnAppearing();

        }

        private void seeMaps_Tapped(object sender, EventArgs e)
        {

        }

        private void btnMessage_Clicked(object sender, EventArgs e)
        {

        }
    }
}