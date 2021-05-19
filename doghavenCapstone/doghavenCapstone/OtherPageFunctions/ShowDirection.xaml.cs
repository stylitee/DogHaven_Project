using Acr.UserDialogs;
using doghavenCapstone.Services;
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
    public partial class ShowDirection : ContentPage
    {
        public ShowDirection()
        {
            InitializeComponent();
            loadDirection();
        }

        private async void loadDirection()
        {
            try
            {
                var pathContent = await InitiateDirection.LoadRoute();

                map.Polylines.Clear();
                var polyline = new Polyline();
                polyline.StrokeColor = Color.Black;
                polyline.StrokeWidth = 3;
                foreach (var c in pathContent)
                {
                    polyline.Positions.Add(c);
                }

                map.Polylines.Add(polyline);

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(polyline.Positions[0].Latitude, polyline.Positions[0].Longitude), Xamarin.Forms.GoogleMaps.Distance.FromKilometers(0.50f)));
                var pin = new Xamarin.Forms.GoogleMaps.Pin
                {
                    Type = PinType.SearchResult,
                    Position = new Xamarin.Forms.GoogleMaps.Position(polyline.Positions.First().Latitude, polyline.Positions.First().Longitude),
                    Label = "Pin",
                    Address = "Pin",
                    Tag = "CirclePoint"
                };

                map.Pins.Add(pin);
                var positionindex = 1;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (pathContent.Count > positionindex)
                    {
                        UpdatePositions(pathContent[positionindex]);
                        positionindex++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                });
            }
            catch (Exception)
            {
                throw;
            }   
        }

        private void UpdatePositions(Position position)
        {
            if(map.Pins.Count == 1 && map.Polylines != null && map.Polylines?.Count > 1)
                return;
            var cPin = map.Pins.FirstOrDefault();
            if(cPin != null)
            {
                cPin.Position = new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(cPin.Position, Distance.FromMeters(200)));
                var previousPosition = map.Polylines?.FirstOrDefault()?.Positions?.FirstOrDefault();
                map.Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition.Value);
            }
            else
            {
                map.Polylines?.FirstOrDefault()?.Positions?.Clear();
            }
        }
    }
}