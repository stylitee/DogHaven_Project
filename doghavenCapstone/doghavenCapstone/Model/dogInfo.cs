using doghavenCapstone.MainPages;
using doghavenCapstone.OtherPageFunctions;
using Newtonsoft.Json;
using System;
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
        [JsonProperty(PropertyName = "createdAt")]
        public DateTimeOffset createdAt { get; set; }
        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset updatedAt { get; set; }

        public dogInfo()
        {
            if(App._updateflag == true && App.uploadFlag == 1)
            {
                NewPageCommand = new Command(GoToThisPage);
            }
        }

        public void GoToThisPage()
        {
            if(App.uploadFlag == 1)
            {
                App.dog_id = id;
                BreedMatchingPage.breedingContentPage[0].Navigation.PushAsync(new DogInformationPage());
            }
            if (App.doginfo_flag == 0)
            {
                App.dog_id = id;
                ProfilePage.profilePage[ProfilePage.profilePage.Count - 1].Navigation.PushAsync(new DogUpdateInfo());
            }
        }
        public ICommand NewPageCommand { get; }
        public string usersDistance { get; set; }

        public static async void Update(dogInfo dogUpdatess)
        {
            await App.client.GetTable<dogInfo>().UpdateAsync(dogUpdatess);
        }

    }
}
