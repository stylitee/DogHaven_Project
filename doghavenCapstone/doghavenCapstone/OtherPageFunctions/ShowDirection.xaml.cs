using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void loadDirection()
        {
            map.Polylines.Clear();
            var polylines = new Xamarin.Forms.GoogleMaps.Polyline();
            polylines.StrokeColor = Color.Black;
            polylines.StrokeWidth = 3;

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(Double.Parse(EstablishmentDetails.latitude), Double.Parse(EstablishmentDetails.longtitude)), Xamarin.Forms.GoogleMaps.Distance.FromKilometers(0.50f)));

        }
    }
}