using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace doghavenCapstone.Model
{
    public class ConversationList
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "user_idOne")]
        public string user_idOne { get; set; }
        [JsonProperty(PropertyName = "user_idTwo")]
        public string user_idTwo { get; set; }
        [JsonProperty(PropertyName = "channelID")]
        public string channelID { get; set; }
    }
}