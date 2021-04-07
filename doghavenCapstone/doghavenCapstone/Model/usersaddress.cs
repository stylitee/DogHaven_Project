using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class usersaddress
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "streetname")]
        public string streetname { get; set; }
        [JsonProperty(PropertyName = "barangay")]
        public string barangay { get; set; }
    }
}
