using Acr.UserDialogs;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace doghavenCapstone.Services
{
    public class InitiateDirection
    {
        public static async Task<System.Collections.Generic.List<Xamarin.Forms.GoogleMaps.Position>> LoadRoute()
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
                    string user_latitude = location.Latitude.ToString();
                    string user_longtitude = location.Longitude.ToString();
                    var googleDirection = await APIServices.ServiceClientInstance.GetDirections(user_latitude, user_longtitude, EstablishmentDetails.latitude, EstablishmentDetails.longtitude);
                    if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                    {
                        var position = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                        return position;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "No billing account linked", "Okay");
                        return null;
                    }
                }
                
            }
            catch (Xamarin.Essentials.PermissionException)
            {
                await App.Current.MainPage.DisplayAlert("Ops", "We need your permission to use this feature", "Okay");
            }
            return null;
        }
    }
}
