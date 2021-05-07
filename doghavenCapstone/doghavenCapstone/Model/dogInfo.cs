using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
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
        public string purposeDesc { get; set; }

        public ICommand NewPageCommand { get; set; }
        public ICommand updateDog { get; set; }

        public dogInfo()
        {
            NewPageCommand = new Command(GoToThisPage);
            updateDog = new Command(dogUpdate);
        }

        

        public void GoToThisPage()
        {
            App.dog_id = id;
            App.dog_name = dogName;
            App.dog_image = dogImage;
            App.dog_gender = dogGender;
            App.dog_purposeID = dogPurpose_id;
            App.dog_breedID = dogBreed_id;
            App.dog_userID = userid;
            BreedMatchingPage.breedingContentPage[0].Navigation.PushAsync(new DogInformationPage());
        }

        public void dogUpdate()
        {
            App.dog_id = id;
            App.dog_name = dogName;
            App.dog_image = dogImage;
            App.dog_gender = dogGender;
            App.dog_purposeID = dogPurpose_id;
            App.dog_breedID = dogBreed_id;
            App.dog_userID = userid;
            ProfilePage.profilePage[ProfilePage.profilePage.Count - 1].Navigation.PushAsync(new DogUpdateInfo());
        }

        public static async void Update(dogInfo dogs)
        {
            await App.client.GetTable<dogInfo>().UpdateAsync(dogs);
        }
    }
}
