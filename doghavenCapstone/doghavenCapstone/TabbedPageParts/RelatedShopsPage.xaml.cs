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
            LoadPlaces();
        }

        private async void LoadPlaces()
        {
            var establishments = await App.client.GetTable<dogRelatedEstablishments>().ToListAsync();
            foreach(var c in establishments)
            {
                string finalimage = "";
                if(c.shopImage == "")
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
                if(c.rate != "0")
                {
                    for (int i = 0; i < Convert.ToInt32(c.rate); i++)
                    {
                        star_rate = star_rate + "★";
                        counter++;
                    }
                    star_rate = "Rate: " + star_rate;
                }
                
                if(counter != 5)
                {
                    int g = 5 - counter;
                    for(int i = 0; i < g; i++)
                    {
                        star_rate = star_rate + " - ";
                    }
                }

                if(c.rate == "0")
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