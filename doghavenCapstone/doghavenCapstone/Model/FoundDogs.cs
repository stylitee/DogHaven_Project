using doghavenCapstone.ClassHelper;
using doghavenCapstone.TabbedPageParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class FoundDogs
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "found_date")]
        public string found_date { get; set; }
        [JsonProperty(PropertyName = "found_time")]
        public string found_time { get; set; }
        [JsonProperty(PropertyName = "placeFound_longtitude")]
        public string placeFound_longtitude { get; set; }
        [JsonProperty(PropertyName = "placeFound_latitude")]
        public string placeFound_latitude { get; set; }
        [JsonProperty(PropertyName = "dogInfo_id")]
        public string dogInfo_id { get; set; }

        //other components
        public string fullName { get; set; }
        public string dogImageSouce { get; set; }
        public string breedName { get; set; }
        public string dateLost { get; set; }
        public string timeLost { get; set; }
        public string placeLost { get; set; }

        //Commands

        public ICommand command { get; }

        public FoundDogs()
        {
            if (App.uploadFlag == 1)
            {
                command = new Command(gotoThisPage);
            }
        }

        public void gotoThisPage()
        {
            VariableStorage.found_id = id;
            VariableStorage.found_userid = userid;
            VariableStorage.found_lastSeen_date = found_date;
            VariableStorage.found_lastSeen_time = found_time;
            VariableStorage.found_placeLost_latitude = placeFound_latitude;
            VariableStorage.found_placeLost_longtitude = placeFound_longtitude;
            VariableStorage.found_doginfo_id = dogInfo_id;
            //FoundPage.FoundPageContent[0].Navigation.PushAsync(new LostDogDetails());
        }

        // getting of lost dogs Pin
    }
}
