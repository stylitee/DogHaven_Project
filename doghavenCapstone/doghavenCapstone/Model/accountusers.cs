
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
    }
}
