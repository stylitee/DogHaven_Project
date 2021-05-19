
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
        [PrimaryKey, MaxLength(250)]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userImage")]
        public string userImage { get; set; }
        [JsonProperty(PropertyName = "username")]
        [MaxLength(250)]
        public string username { get; set; }
        [JsonProperty(PropertyName = "userPassword")]
        [MaxLength(250)]
        public string userPassword { get; set; }
        [JsonProperty(PropertyName = "fullName")]
        [MaxLength(250)]
        public string fullName { get; set; }
        [JsonProperty(PropertyName = "address_id")]
        public string address_id { get; set; }
        [JsonProperty(PropertyName = "user_role_id")]
        public string user_role_id { get; set; }
        [JsonProperty(PropertyName = "phoneNumber")]
        public string phoneNumber { get; set; }

        public static async void Update(accountusers usertype)
        {
            await App.client.GetTable<accountusers>().UpdateAsync(usertype);
        }
    }
}
