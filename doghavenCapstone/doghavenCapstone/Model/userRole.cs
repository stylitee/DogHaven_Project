using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class userRole
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "roleDescription")]
        public string roleDescription { get; set; }
    }
}
