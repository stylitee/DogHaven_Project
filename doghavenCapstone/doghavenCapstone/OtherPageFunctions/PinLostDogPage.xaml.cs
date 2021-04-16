using doghavenCapstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            loadUserLocation();
        }

        private async void loadUserLocation()
        {
            string latitude = "", longtitude = "";
            var getLocation = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == App.user_id).ToListAsync();
            foreach (var row in getLocation)
            {
                latitude = row.latitude;
                longtitude = row.longtitude;
            }

            Pin pinMyAddress = new Pin()
            {
                Type = PinType.Place,
                Label = "Naga City",
                Address = "26-7 Looban 6 Bayawas St. Barangay Abella",
                Position = new Position(double.Parse(latitude), double.Parse(longtitude)),
                Rotation = 33.3f,
                Tag = "id_Bayawas"
            };

            lostMaps.Pins.Add(pinMyAddress);
            lostMaps.MoveToRegion(MapSpan.FromCenterAndRadius(pinMyAddress.Position, Distance.FromMeters(5000)));

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
    }
}