using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class dogInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "dogName")]
        public string dogName { get; set; }
        [JsonProperty(PropertyName = "dogImage")]
        public string dogImage { get; set; }
        [JsonProperty(PropertyName = "dogGender")]
        public string dogGender { get; set; }
        [JsonProperty(PropertyName = "dogPurpose_id")]
        public string dogPurpose_id { get; set; }
        [JsonProperty(PropertyName = "dogBreed_id")]
        public string dogBreed_id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }

        public string breed_Name { get; set; }
    }
}
