using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class DogPrices
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "doginfo_id")]
        public string doginfo_id { get; set; }
        [JsonProperty(PropertyName = "price")]
        public string price { get; set; }
        [JsonProperty(PropertyName = "withCompletePapers")]
        public string withCompletePapers { get; set; }
        [JsonProperty(PropertyName = "completeVaccines")]
        public string completeVaccines { get; set; }
        [JsonProperty(PropertyName = "isPuppy")]
        public string isPuppy { get; set; }
        [JsonProperty(PropertyName = "Age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "seller_id")]
        public string seller_id { get; set; }

        //walagn seller id sa database lagyan mo pag balik ng internet
    }
}
