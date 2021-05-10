using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class dogMatches
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "dog1")]
        public string dog1 { get; set; }
        [JsonProperty(PropertyName = "dog2")]
        public string dog2 { get; set; }
        [JsonProperty(PropertyName = "markAsDone")]
        public string markAsDone { get; set; }
    }
}
