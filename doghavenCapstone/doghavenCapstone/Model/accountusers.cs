
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace doghavenCapstone.Model
{
    public class accountusers
    {
        [JsonProperty(PropertyName = "id")]
        [PrimaryKey,AutoIncrement]
        public string id { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }
        [JsonProperty(PropertyName = "userPassword")]
        public string userPassword { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string firstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string lastName { get; set; }
        [JsonProperty(PropertyName = "middleName")]
        public string middleName { get; set; }
        [JsonProperty(PropertyName = "address_id")]
        public string address_id { get; set; }
        [JsonProperty(PropertyName = "user_role_id")]
        public string user_role_id { get; set; }
    }
}
