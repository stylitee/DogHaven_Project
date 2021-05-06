using doghavenCapstone.ClassHelper;
using doghavenCapstone.DetailsPage;
using doghavenCapstone.TabbedPageParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class dogSeller
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }
        [JsonProperty(PropertyName = "isRegistered")]
        public string isRegistered { get; set; }

        //unrelated table properties

        public string fullName { get; set; }
        public string dogsOwnedForSelling { get; set; }
        public string breedsName { get; set; }
        public string sellerImage { get; set; }

        //Commands


        public ICommand SeeDetails { get; set; }
        

        public dogSeller()
        {
            if(App.uploadFlag == 1)
            {
                SeeDetails = new Command(GoToThisPage);
            }
        }

        public void GoToThisPage()
        {
            
            VariableStorage.seller_id = id;
            VariableStorage.sellersUser_id = userid;
            VariableStorage.SellersisRegistered = isRegistered;
            DogSellerPage._DogSellerPage[DogSellerPage._DogSellerPage.Count - 1].Navigation.PushAsync(new SellerDetails());
            DogSellerPage._DogSellerPage.Clear();
        }
    }
}
