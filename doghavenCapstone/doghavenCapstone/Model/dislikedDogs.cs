using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class dislikedDogs
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "dog_id")]
        public string dog_id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }

        public static async void Insert(dislikedDogs dis_like)
        {
            await App.client.GetTable<dislikedDogs>().InsertAsync(dis_like);
        }
    }
}
