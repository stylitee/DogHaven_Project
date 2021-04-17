using doghavenCapstone.ClassHelper;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.TabbedPageParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class LostDogs
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "lastSeen_date")]
        public string lastSeen_date { get; set; }
        [JsonProperty(PropertyName = "lastSeen_time")]
        public string lastSeen_time { get; set; }
        [JsonProperty(PropertyName = "placeLost_longtitude")]
        public string placeLost_longtitude { get; set; }
        [JsonProperty(PropertyName = "placeLost_latitude")]
        public string placeLost_latitude { get; set; }
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

        public ICommand command { get; set; }

        public LostDogs()
        {
            if(App.uploadFlag == 1)
            {
                command = new Command(gotoThisPage);
            }
        }

        public void gotoThisPage()
        {
            VariableStorage.id = id;
            VariableStorage.userid = userid;
            VariableStorage.lastSeen_date = lastSeen_date;
            VariableStorage.lastSeen_time = lastSeen_time;
            VariableStorage.placeLost_latitude = placeLost_latitude;
            VariableStorage.placeLost_longtitude = placeLost_longtitude;
            VariableStorage.doginfo_id = dogInfo_id;
            LostPage.LostPageContent[0].Navigation.PushAsync(new LostDogDetails());
        }

        // getting of lost dogs Pin

    }
}
