using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class LostDogs
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "lastSeen_date")]
        public string lastSeen_date { get; set; }
        [JsonProperty(PropertyName = "lastSeen_time")]
        public string lastSeen_time { get; set; }
        [JsonProperty(PropertyName = "placeLost_longtitude")]
        public string placeLost_longtitude { get; set; }
        [JsonProperty(PropertyName = "placeLost_latitude")]
        public string placeLost_latitude { get; set; }
        [JsonProperty(PropertyName = "dogInfo_id")]
        public string dogInfo_id { get; set; }
    }
}
