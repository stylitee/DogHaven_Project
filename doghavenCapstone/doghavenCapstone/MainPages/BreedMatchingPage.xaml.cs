using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
using MLToolkit.Forms.SwipeCardView;
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
        public static List<dogInfo> lstDogs = new List<dogInfo>();
        public static List<string> dogId = new List<string>();

        public BreedMatchingPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public async void loadDogs()
        {
            _Doglist.Clear();
            dogId.Clear();
            var likedTable = await App.client.GetTable<likedDogs>().ToListAsync();
            var dislikedTable = await App.client.GetTable<dislikedDogs>().ToListAsync();
            List<dogInfo> dogInfoTable = new List<dogInfo>();
            string breed_Name = "";

            if(likedTable.Count == 0 && dislikedTable.Count == 0)
            {
                dogInfoTable = await App.client.GetTable<dogInfo>().Where(x => x.userid != App.user_id).ToListAsync();

                foreach(var dog in dogInfoTable)
                {
                    var breedName = await App.client.GetTable<dogBreed>().Where(breed => breed.id == dog.dogBreed_id).ToListAsync();

                    foreach (var b in breedName)
                    {
                        breed_Name = b.breedName;
                    }
                    System.Uri url = new System.Uri(dog.dogImage);
                    _Doglist.Add(new dogInfo()
                    {
                        dogName = dog.dogName,
                        dogGender = dog.dogGender,
                        breed_Name = breed_Name,
                        dogImage = dog.dogImage
                    });
                    dogId.Add(dog.id);
                }
            }

            if(likedTable.Count != 0 || dislikedTable.Count != 0)
            {
                List<dogInfo> finalList = new List<dogInfo>();
                List<likedDogs> _likedOthers = new List<likedDogs>();
                List<likedDogs> lstLiked = new List<likedDogs>();
                var getLikedInfo = await App.client.GetTable<likedDogs>().Where(x => x.userid == App.user_id).ToListAsync();
                var getdisLikedInfo = await App.client.GetTable<dislikedDogs>().Where(x => x.userid == App.user_id).ToListAsync();
                
                foreach(var c in getLikedInfo)
                {
                    dogInfoTable = await App.client.GetTable<dogInfo>().Where(x => x.id != c.dog_id && x.userid != c.userid).ToListAsync();
                    foreach(var g in dogInfoTable)
                    {
                        finalList.Add(g);
                    }
                    dogInfoTable.Clear();
                }
                foreach(var d in getdisLikedInfo)
                {
                    dogInfoTable = await App.client.GetTable<dogInfo>().Where(x => x.id != d.dog_id && x.userid != d.userid).ToListAsync();
                    foreach (var g in dogInfoTable)
                    {
                        finalList.Add(g);
                    }
                    dogInfoTable.Clear();
                }

                dogInfoTable = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
                foreach(var d in dogInfoTable)
                {
                    var _liked = await App.client.GetTable<likedDogs>().Where(x => x.userid != App.user_id && x.dog_id == d.id).ToListAsync();
                    foreach(var c in _liked)
                    {
                        _likedOthers.Add(c);
                    }
                }
                dogInfoTable.Clear();

                foreach(var likeby in _likedOthers)
                {
                    var _addition = await App.client.GetTable<dogInfo>().Where(x => x.id == likeby.dog_id).ToListAsync();
                    foreach(var final in _addition)
                    {
                        finalList.Add(final);
                    }
                }
                _likedOthers.Clear();                
                //makua ning mga rows sa likedTable
                foreach (var dog in finalList)
                {
                    var breedName = await App.client.GetTable<dogBreed>().Where(breed => breed.id == dog.dogBreed_id).ToListAsync();

                    foreach (var b in breedName)
                    {
                        breed_Name = b.breedName;
                    }
                    System.Uri url = new System.Uri(dog.dogImage);
                    _Doglist.Add(new dogInfo()
                    {
                        dogName = dog.dogName,
                        dogGender = dog.dogGender,
                        breed_Name = breed_Name,
                        dogImage = dog.dogImage
                    });
                    dogId.Add(dog.id);
                }
                finalList.Clear();
            }
            //ang pag remove item sa list sa swiping na
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
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadDogs();
        }

        private void btnNope_Clicked(object sender, EventArgs e)
        {
            swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left);
        }

        private void btnSuperLike_Clicked(object sender, EventArgs e)
        {
            swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Up);
        }

        private void btnLike_Clicked(object sender, EventArgs e)
        {
            swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UploadDogPage());
        }

        public async void swipeLeftAlgo()
        {
            try
            {
                await swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left);
                dislikedDogs dis_like = new dislikedDogs()
                {
                    id = Id.ToString("N").Substring(0, 10),
                    userid = App.user_id,
                    dog_id = dogId[0]
                };

                dislikedDogs.Insert(dis_like);
                Acr.UserDialogs.UserDialogs.Instance.Toast("You disliked the dog", new TimeSpan(2));
                if(_Doglist.Count != 0 && dogId.Count != 0)
                {
                    _Doglist.RemoveAt(0);
                    dogId.RemoveAt(0);
                }
                if(_Doglist.Count == 0 && dogId.Count == 0)
                {

                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async void swipeRightAlgo()
        {
            try
            {
                await swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
                likedDogs liked = new likedDogs()
                {
                    id = Id.ToString("N").Substring(0, 10),
                    userid = App.user_id,
                    dog_id = dogId[0]
                };

                likedDogs.Insert(liked);
                Acr.UserDialogs.UserDialogs.Instance.Toast("You liked the dog", new TimeSpan(2));
                if (_Doglist.Count != 0 && dogId.Count != 0)
                {
                    _Doglist.RemoveAt(0);
                    dogId.RemoveAt(0);
                }
                if (_Doglist.Count == 0 && dogId.Count == 0)
                {

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            //left
            swipeLeftAlgo();
        }

        private void SwipeGestureRecognizer_Swiped_1(object sender, SwipedEventArgs e)
        {
            //right
            try
            {
                swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
                Acr.UserDialogs.UserDialogs.Instance.Toast("You liked the dog", new TimeSpan(3));
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SwipeGestureRecognizer_Swiped_2(object sender, SwipedEventArgs e)
        {
            //up
            try
            {
                swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Up);
                Acr.UserDialogs.UserDialogs.Instance.Toast("You super liked the dog", new TimeSpan(3));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}