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
    public class dogRelatedEstablishments
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "shopImage")]
        public string shopImage { get; set; }
        [JsonProperty(PropertyName = "nameOfShop")]
        public string nameOfShop { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public string latitude { get; set; }
        [JsonProperty(PropertyName = "longtitude")]
        public string longtitude { get; set; }
        [JsonProperty(PropertyName = "rate")]
        public string rate { get; set; }
        [JsonProperty(PropertyName = "addtionalDesc")]
        public string addtionalDesc { get; set; }
        [JsonProperty(PropertyName = "facebookLink")]
        public string facebookLink { get; set; }

        public ICommand NewPageCommand { get; }

        public dogRelatedEstablishments()
        {
            NewPageCommand = new Command(GoToThisPage);
        }

        private void GoToThisPage()
        {
            RelatedShopsPage.store_id = id;
            RelatedShopsPage.cnt[0].Navigation.PushAsync(new EstablishmentDetails());
        }
    }
}
