using Acr.UserDialogs;
using doghavenCapstone.MainPages;
using doghavenCapstone.OtherPageFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.Model
{
    public class dogInfo 
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "dogName")]
        public string dogName { get; set; }
        [JsonProperty(PropertyName = "dogImage")]
        public string dogImage { get; set; }
        [JsonProperty(PropertyName = "dogGender")]
        public string dogGender { get; set; }
        [JsonProperty(PropertyName = "dogBreed_id")]
        public string dogBreed_id { get; set; }
        [JsonProperty(PropertyName = "dogPurpose_id")]
        public string dogPurpose_id { get; set; }
        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }

        public string breed_Name { get; set; }
        public string usersDistance { get; set; }
        public dogInfo()
        {
            if(App.uploadFlag == 1)
            {
                NewPageCommand = new Command(GoToThisPage);
            }
        }

        public ICommand NewPageCommand { get; set; }

        public void GoToThisPage()
        {
            App.dog_id = id;
            App.dog_name = dogName;
            App.dog_image = dogImage;
            App.dog_gender = dogGender;
            App.dog_purposeID = dogPurpose_id;
            App.dog_breedID = dogBreed_id;
            App.dog_userID = userid;
            BreedMatchingPage.breedingContentPage[BreedMatchingPage.breedingContentPage.Count - 1].Navigation.PushAsync(new DogInformationPage());
            BreedMatchingPage.breedingContentPage.Clear();
        }
    }
}
