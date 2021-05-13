using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllLostDogsMap : ContentPage
    {
        LostDogs dogsList;
        public AllLostDogsMap()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            BindingContext = dogsList = new LostDogs();
            LoadMapPins();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void LoadMapPins()
        {
            try
            {
                if(VariableStorage.lostAndFoundIdentifier == "Lost")
                {
                    var contents = await App.client.GetTable<LostDogs>().ToListAsync();

                    if (contents != null)
                    {
                        string desc = "";
                        foreach (var item in contents)
                        {
                            var users = await App.client.GetTable<accountusers>().Where(x => x.id == item.userid).ToListAsync();
                            foreach (var c in users)
                            {
                                desc = "Owned by: " + c.fullName;
                            }
                            Pin pinLostDogs = new Pin()
                            {
                                Label = desc + Environment.NewLine +
                                "Date Lost: " + item.dateLost,
                                Type = PinType.Place,
                                Position = new Position(double.Parse(item.placeLost_latitude), double.Parse(item.placeLost_longtitude)),
                                Rotation = 33.3f
                            };

                            ShowAlllostMaps.Pins.Add(pinLostDogs);
                        }

                        var location = await Geolocation.GetLastKnownLocationAsync();
                        if (location == null)
                        {
                            location = await Geolocation.GetLocationAsync(new GeolocationRequest
                            {
                                DesiredAccuracy = GeolocationAccuracy.Medium,
                                Timeout = TimeSpan.FromSeconds(30)
                            });
                        }
                        var myPosition = new Position(location.Latitude, location.Longitude);

                        ShowAlllostMaps.MoveToRegion(MapSpan.FromCenterAndRadius(myPosition, Distance.FromMeters(1000)));
                    }
                }
                else
                {
                    var contents = await App.client.GetTable<FoundDogs>().ToListAsync();

                    if (contents != null)
                    {
                        string desc = "";
                        foreach (var item in contents)
                        {
                            var users = await App.client.GetTable<accountusers>().Where(x => x.id == item.userid).ToListAsync();
                            foreach (var c in users)
                            {
                                desc = "Owned by: " + c.fullName;
                            }
                            Pin pinLostDogs = new Pin()
                            {
                                Label = desc + Environment.NewLine +
                                "Date Lost: " + item.dateLost,
                                Type = PinType.Place,
                                Position = new Position(double.Parse(item.placeFound_latitude), double.Parse(item.placeFound_longtitude)),
                                Rotation = 33.3f
                            };

                            ShowAlllostMaps.Pins.Add(pinLostDogs);
                        }

                        var location = await Geolocation.GetLastKnownLocationAsync();
                        if (location == null)
                        {
                            location = await Geolocation.GetLocationAsync(new GeolocationRequest
                            {
                                DesiredAccuracy = GeolocationAccuracy.Medium,
                                Timeout = TimeSpan.FromSeconds(30)
                            });
                        }
                        var myPosition = new Position(location.Latitude, location.Longitude);

                        ShowAlllostMaps.MoveToRegion(MapSpan.FromCenterAndRadius(myPosition, Distance.FromMeters(1000)));
                    }
                }
                
            }
            catch (Xamarin.Essentials.PermissionException)
            {
                await DisplayAlert("Ops", "We need your permission to access your location to use the maps", "Okay");
            }
            
        }
    }
}