using doghavenCapstone.API_Keys;
using doghavenCapstone.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace doghavenCapstone.Services
{
    public class APIServices
    {
        public JsonSerializer _serializer = new JsonSerializer();
        public static APIServices _ServiceClientInstance;
        public  static APIServices ServiceClientInstance
        { 
            get
            {
                if (_ServiceClientInstance == null)
                    _ServiceClientInstance = new APIServices();
                return _ServiceClientInstance;
            }
        }

        private HttpClient client;
        public APIServices()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://maps.googleapis.com/maps/");
        }

        public async Task<GoogleDirection> GetDirections(string originLatitude, string originLongtitude, string destinationLatitude, string destinationLongtitude)
        {
            GoogleDirection googleDirection = new GoogleDirection();

            var response = await client.GetAsync($"api/directions/json?mode=driving&transit_routing_preferences=less_driving&origin={originLatitude},{originLongtitude}&destination={destinationLatitude},{destinationLongtitude}&key={GoogleMapsApiKey.Key}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    googleDirection = await Task.Run(() =>
                        JsonConvert.DeserializeObject<GoogleDirection>(json)
                    ).ConfigureAwait(false);
                }
            }
            return googleDirection;
        }
    }
}
