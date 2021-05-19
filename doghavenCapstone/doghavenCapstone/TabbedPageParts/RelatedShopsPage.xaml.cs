using Acr.UserDialogs;
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
    public partial class RelatedShopsPage : ContentPage
    {
        public static List<ContentPage> cnt = new List<ContentPage>();
        public static string store_id = "";
        public ObservableCollection<dogRelatedEstablishments> _listOfEstablishments = new ObservableCollection<dogRelatedEstablishments>();
        public RelatedShopsPage()
        {
            InitializeComponent();
            BindingContext = this;
            cnt.Add(this);
        }

        public double getDistance(double user1_latitude, double user1_longtitude, double user2_latitude, double user2_longitude)
        {
            Location sourceCoordinates = new Location(user1_latitude, user1_longtitude);
            Location destinationCoordinates = new Location(user2_latitude, user2_longitude);
            double distance = Location.CalculateDistance(sourceCoordinates, destinationCoordinates, DistanceUnits.Kilometers);
            return distance;
        }

        private async void LoadPlaces()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location == null)
                {
                    UserDialogs.Instance.Toast("NO GPS", new TimeSpan(1));
                }
                else
                {
                    var establishments = await App.client.GetTable<dogRelatedEstablishments>().ToListAsync();
                    foreach (var c in establishments)
                    {
                        _listOfEstablishments.Clear();
                        string finalimage = "";
                        if (c.shopImage == "")
                        {
                            finalimage = "https://doghaven2storage.blob.core.windows.net/noimage/noimage.jpg";
                        }
                        else
                        {
                            finalimage = c.shopImage;
                        }

                        var placemarks = await Geocoding.GetPlacemarksAsync(double.Parse(c.latitude), double.Parse(c.longtitude));
                        var placemark = placemarks?.FirstOrDefault();

                        var geocodeAddress = placemark.SubThoroughfare + ", " +
                                             placemark.Thoroughfare + ", " +
                                             placemark.Locality + ", " +
                                             placemark.SubAdminArea + ", " +
                                             placemark.AdminArea + ", " +
                                             placemark.CountryName;

                        string star_rate = "";
                        int counter = 0;
                        if (c.rate != "0")
                        {
                            for (int i = 0; i < Convert.ToInt32(c.rate); i++)
                            {
                                star_rate = star_rate + "★";
                                counter++;
                            }
                            star_rate = "Rate: " + star_rate;
                        }

                        if (counter != 5)
                        {
                            int g = 5 - counter;
                            for (int i = 0; i < g; i++)
                            {
                                star_rate = star_rate + " - ";
                            }
                        }

                        if (c.rate == "0")
                        {
                            star_rate = "No ratings available";
                        }

                        _listOfEstablishments.Add(new dogRelatedEstablishments()
                        {
                            id = c.id,
                            shopImage = finalimage,
                            nameOfShop = "Name: " + c.nameOfShop,
                            latitude = geocodeAddress,
                            rate = star_rate,
                        });
                    }
                }

            }
            catch (Xamarin.Essentials.PermissionException)
            {
                await DisplayAlert("Permission Error", "We need to access your location to be able to use this feature", "Okay");

            }
            catch (Exception)
            {
                await DisplayAlert("Ops", "Something went wrong getting your location, make sure your gps is on while connected to the internet", "Okay");
            }
        }

        protected override void OnAppearing()
        {
            LoadPlaces();
            base.OnAppearing();
        }

        public ObservableCollection<dogRelatedEstablishments> listOfEstablishments
        {
            get => _listOfEstablishments;
            set
            {
                _listOfEstablishments = value;
            }
        }

        private void addShop_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddShop());
        }

        private void btnSeeAllEstablishments_Clicked(object sender, EventArgs e)
        {

        }
    }
}