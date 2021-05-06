using doghavenCapstone.ClassHelper;
using doghavenCapstone.DetailsPage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class DogPrice
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "doginfo_id")]
        public string doginfo_id { get; set; }
        [JsonProperty(PropertyName = "price")]
        public string price { get; set; }
        [JsonProperty(PropertyName = "withCompletePapers")]
        public string withCompletePapers { get; set; }
        [JsonProperty(PropertyName = "completeVaccines")]
        public string completeVaccines { get; set; }
        [JsonProperty(PropertyName = "Age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "seller_id")]
        public string seller_id { get; set; }

        //unrelated props

        public string dogImage { get; set; }
        public string dogBreed { get; set; }

        public ICommand SeeDogInfo { get; set; }

        public DogPrice()
        {
            if (App.uploadFlag == 1)
            {
                SeeDogInfo = new Command(DogSellPage);
            }
        }

        public void DogSellPage()
        {
            VariableStorage.dogDetails_id = id;
            VariableStorage.dogDetails_breeedName = dogBreed;
            VariableStorage.dogDetails_age = Age;
            VariableStorage.dogDetails_price = price;
            VariableStorage.dogDetails_vaccine = completeVaccines;
            VariableStorage.dogDetails_completepapers = withCompletePapers;
            VariableStorage.dogDetails_sellerid = seller_id;
            VariableStorage.dogDetails_doginfoID = doginfo_id;
            SellerDetails.dogForsale[SellerDetails.dogForsale.Count - 1].Navigation.PushAsync(new dogForSaleDetails());
            SellerDetails.dogForsale.Clear();
        }

    }
}
