using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class dogSeller
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "isRegistered")]
        public string isRegistered { get; set; }

        //unrelated table properties

        public string fullName { get; set; }
        public string dogsOwnedForSelling { get; set; }
        public string breedsName { get; set; }
        public string sellerImage { get; set; }
    }
}
