using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinLostDogPage : ContentPage
    {
        
        public PinLostDogPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            BindingContext = new LostDogs();
            loadUserLocation();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void loadUserLocation()
        {
            string latitude = "", longtitude = "";

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
                //igdi ma set
                latitude = location.Latitude.ToString();
                longtitude = location.Longitude.ToString();
            }



            Pin pinMyAddress = new Pin()
            {
                Type = PinType.Place,
                Label = "Pin to Lost Place",
                Position = new Position(double.Parse(latitude), double.Parse(longtitude)),
                Rotation = 33.3f,
                IsDraggable = true
            };

            lostMaps.Pins.Add(pinMyAddress);
            lostMaps.MoveToRegion(MapSpan.FromCenterAndRadius(pinMyAddress.Position, Distance.FromMeters(500)));

            ApplyMyMapTheme();
        }

        private void ApplyMyMapTheme()
        {
            var assembly = typeof(PinLostDogPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"doghavenCapstone.MapThemes.MapStyle.json");
            string themefile;

            using(var reader = new System.IO.StreamReader(stream))
            {
                themefile = reader.ReadToEnd();
                lostMaps.MapStyle = MapStyle.FromJson(themefile);
            }
        }

        private void lostMaps_PinDragStart(object sender, PinDragEventArgs e)
        {

        }

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void lostMaps_PinDragEnd(System.Object sender, Xamarin.Forms.GoogleMaps.PinDragEventArgs e)
        {
            if(VariableStorage.lostAndFoundIdentifier == "Lost")
            {
                AddLostDogPage.setLocation_latitude = e.Pin.Position.Latitude.ToString();
                AddLostDogPage.setLocation_longtitude = e.Pin.Position.Longitude.ToString();
                AddLostDogPage.lbl[0].Text = "location is pinned succesfully";
            }
            else if(VariableStorage.lostAndFoundIdentifier == "Found")
            {
                AddFoundDogPage.setLocation_latitude = e.Pin.Position.Latitude.ToString();
                AddFoundDogPage.setLocation_longtitude = e.Pin.Position.Longitude.ToString();
                AddFoundDogPage.lbl[0].Text = "location is pinned succesfully";
            }
            else
            {
                AddShop.latitude = e.Pin.Position.Latitude.ToString();
                AddShop.longtitude = e.Pin.Position.Longitude.ToString();
                AddShop.lbl[0].Text = "Location pinned successfully";
            }
        }
    }
}