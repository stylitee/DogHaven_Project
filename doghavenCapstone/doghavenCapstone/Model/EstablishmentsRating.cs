using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.TabbedPageParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class EstablishmentsRating
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "establishment_id")]
        public string establishment_id { get; set; }
        [JsonProperty(PropertyName = "rating")]
        public string rating { get; set; }
    }
}
