using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class getCurrentLocation
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string user_id { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public string latitude { get; set; }
        [JsonProperty(PropertyName = "longtitude")]
        public string longtitude { get; set; }
    }
}
