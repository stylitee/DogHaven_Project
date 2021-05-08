using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class usersaddress
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "streetname")]
        public string streetname { get; set; }
        [JsonProperty(PropertyName = "barangay")]
        public string barangay { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }
        [JsonProperty(PropertyName = "province")]
        public string province { get; set; }
    }
}
