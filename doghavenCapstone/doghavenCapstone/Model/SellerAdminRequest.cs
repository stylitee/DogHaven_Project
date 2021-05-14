using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class SellerAdminRequest
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string user_id { get; set; }
        [JsonProperty(PropertyName = "valid_id")]
        public string valid_id { get; set; }
        [JsonProperty(PropertyName = "licence_id")]
        public string licence_id { get; set; }
        [JsonProperty(PropertyName = "admin_response")]
        public string admin_response{ get; set; }
    }
}
