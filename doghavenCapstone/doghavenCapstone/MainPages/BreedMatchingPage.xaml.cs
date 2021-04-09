using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreedMatchingPage : ContentPage
    {
        public ObservableCollection<dogInfo> _Doglist = new ObservableCollection<dogInfo>();
        public BreedMatchingPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public async void loadDogs()
        {
            var mydogList = await App.client.GetTable<dogInfo>().ToListAsync();
            string bName = "";
            if(mydogList.Count == 0 || mydogList == null)
            {
                return;
            }
            else
            {
                foreach (var dog in mydogList)
                {
                    var breedName = await App.client.GetTable<dogBreed>().Where(breed => breed.id == dog.dogBreed_id).ToListAsync();
                    foreach (var b in breedName)
                    {
                        bName = b.breedName;
                    }
                    System.Uri url = new System.Uri(dog.dogImage);
                    _Doglist.Add(new dogInfo()
                    {
                        dogName = dog.dogName,
                        dogGender = dog.dogGender,
                        breed_Name = bName,
                        dogImage = dog.dogImage
                    });
                }
            }
        }

        public ObservableCollection<dogInfo> DogList
        {
            get => _Doglist;
            set
            {
                _Doglist = value;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UploadDogPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadDogs();
        }

        private void btnNope_Clicked(object sender, EventArgs e)
        {

        }

        private void btnSuperLike_Clicked(object sender, EventArgs e)
        {

        }

        private void btnLike_Clicked(object sender, EventArgs e)
        {

        }
    }
}