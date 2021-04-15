using Acr.UserDialogs;
using doghavenCapstone.InitialPages;
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
    public partial class GetUsersLocation : ContentPage
    {
        public GetUsersLocation()
        {
            InitializeComponent();
            var assembly = typeof(GetUsersLocation);
            UserDialogs.Instance.HideLoading();
            imgLocationAnimation.Source = ImageSource.FromResource("doghavenCapstone.Assets.get_location.gif", assembly);
        }

        private async void btnGetLocation_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if(location == null)
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
                    getCurrentLocation mylocation = new getCurrentLocation()
                    {
                        id = Guid.NewGuid().ToString("N").Substring(0, 15),
                        user_id = App.user_id, 
                        latitude = location.Latitude.ToString(),
                        longtitude = location.Longitude.ToString()
                    };

                    await App.client.GetTable<getCurrentLocation>().InsertAsync(mylocation);
                    await Navigation.PushAsync(new NewAccountVerify());
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops","Something went wrong getting your location, make sure your gps is on while connected to the internet","Okay");
            }
        }
    }
}