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
        public int flag = 0;
        public BreedMatchingPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Command LeftCommand => new Command<string>(LeftSwipe);

        private void LeftSwipe(string parameter)
        {
            Acr.UserDialogs.UserDialogs.Instance.Toast("You disliked the dog" + parameter, new TimeSpan(2));
        }

        public async void loadDogs()
        {
            int iterate = 0;
            if(flag == 0)
            {
                var mydogList = await App.client.GetTable<dogInfo>().Where(d => d.userid != App.user_id).ToListAsync();
                foreach (var dog in mydogList)
                {
                    if (iterate <= 15)
                    {
                        lstDogs.Add(dog);
                        iterate++;
                    }
                }
            }
            else
            {
                var mydogList = await App.client.GetTable<dogInfo>().Skip(flag).Where(d => d.userid != App.user_id).ToListAsync();
                foreach (var dog in mydogList)
                {
                    if (iterate <= 15)
                    {
                        lstDogs.Add(dog);
                        iterate++;
                    }
                }
            }
            flag = flag + iterate;
            iterate = 0;
            string bName = "";
            if(lstDogs.Count == 0 || lstDogs == null)
            {
                return;
            }
            else
            {
                //hahalion dgdi si nasa dislike
                int myindex = 0;
                List<int> index = new List<int>();
                foreach(var c in lstDogs)
                {
                    var removeDislikeDogs = await App.client.GetTable<dislikedDogs>().Where(d => d.dog_id == c.id && d.userid == App.user_id).ToListAsync();
                    foreach (var d in removeDislikeDogs)
                    {
                        if (c.id == d.dog_id)
                        {
                            index.Add(myindex);
                        }
                        myindex++;
                    }
                }
                for (int i = 0; i < index.Count - 1; i++)
                {
                    lstDogs.RemoveAt(i);
                }
                foreach (var dog in lstDogs)
                {
                    
                    //lstDogs.RemoveAt(0);
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
                    dogId.Add(dog.id);
                }
                myindex = 0;
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

                await App.client.GetTable<dislikedDogs>().InsertAsync(dis_like);
                Acr.UserDialogs.UserDialogs.Instance.Toast("You disliked the dog", new TimeSpan(2));
                _Doglist.RemoveAt(0);
                dogId.RemoveAt(0);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /*private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            //left
            swipeLeftAlgo();
        }*/

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