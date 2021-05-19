using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
using doghavenCapstone.LocalDBModel;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using doghavenCapstone.PreventerPage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreedMatchingPage : ContentPage
    {
        public ObservableCollection<dogInfo> _Doglist = new ObservableCollection<dogInfo>();
        public static List<ContentPage> breedingContentPage = new List<ContentPage>();
        public static List<dogInfo> lstDogs = new List<dogInfo>();
        public static List<string> dogId = new List<string>();
        List<dogInfo> pckrID = new List<dogInfo>();
        List<dogInfo> dogInfoTable = new List<dogInfo>();
        List<dogInfo> _mydoglist = new List<dogInfo>();
        List<string> _breedNameList = new List<string>();
        List<string> _breedIdList = new List<string>();
        double user_latitude = 0, user_longtitude = 0, otherUser_latitude = 0, otherUser_longtitude = 0;
        public BreedMatchingPage()
        { 

            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            BindingContext = this;
            App.buttonName = "Back";
            loadYourDogs();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void loadYourDogs()
        {
            pckrMatchType.Items.Clear();
            _mydoglist.Clear();
            pckrDogList.Items.Clear();
            _breedNameList.Clear();

            pckrMatchType.Items.Add("Random");
            pckrMatchType.Items.Add("Pure Breed");
            pckrMatchType.SelectedIndex = 0;
            var myDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
            if (myDogs.Count != 0)
            {
                foreach (var dogs in myDogs)
                {
                    var purpose = await App.client.GetTable<dogPurpose>().Where(x => x.id == dogs.dogPurpose_id).ToListAsync();
                    if(purpose[0].dogDesc == "Breeding")
                    {
                        pckrDogList.Items.Add(dogs.dogName);
                        pckrID.Add(dogs);
                        _mydoglist.Add(dogs);
                        var bred = await App.client.GetTable<dogBreed>().Where(x => x.id == dogs.dogBreed_id).ToListAsync();
                        foreach (var c in bred)
                        {
                            _breedNameList.Add(c.breedName);
                            _breedIdList.Add(c.id);
                        }
                    }
                }
            }
            else
            {
                pckrDogList.Items.Add("No dogs available");
                pckrDogList.SelectedIndex = 0;
            }
            
        }



        public async void getUserLocation()
        {
            var loc = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == App.user_id).ToListAsync();
            foreach(var longti in loc)
            {
                user_latitude = double.Parse(longti.latitude);
                user_longtitude = double.Parse(longti.longtitude);
            }
        }

        public double getDistance(double user1_latitude, double user1_longtitude, double user2_latitude, double user2_longitude)
        {
            Location sourceCoordinates = new Location(user1_latitude, user1_longtitude);
            Location destinationCoordinates = new Location(user2_latitude, user2_longitude);
            double distance = Location.CalculateDistance(sourceCoordinates, destinationCoordinates, DistanceUnits.Kilometers);
            return distance;
        }
        public async void loadDogs()
        {
            //bool startDisplay = false;
            UserDialogs.Instance.ShowLoading("Gathering dogs in your area...");
            _Doglist.Clear();
            dogId.Clear();
            var likedTable = await App.client.GetTable<likedDogs>().ToListAsync();
            var dislikedTable = await App.client.GetTable<dislikedDogs>().ToListAsync();
            getUserLocation();
            string breed_Name = "";

            //pag mayo pa laman anng liked and disliked table
            if (likedTable.Count == 0 && dislikedTable.Count == 0)
            {
                int flag = 0;
                dogInfoTable = await App.client.GetTable<dogInfo>().Where(x => x.userid != App.user_id && x.dogPurpose_id == "dk2emn1ik").ToListAsync();
                foreach (var c in dogInfoTable)
                {
                    if(flag != dogInfoTable.Count())
                    {
                        flag++;
                        continue;
                    }
                    else
                    {
                        flag++;
                    }
                }
                
                if(flag == dogInfoTable.Count())
                {
                    foreach (var dog in dogInfoTable)
                    {
                        var getLocation = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == dog.userid).ToListAsync();
                        foreach(var c in getLocation)
                        {
                            otherUser_latitude = double.Parse(c.latitude);
                            otherUser_longtitude = double.Parse(c.longtitude);
                        }
                        
                        var breedName = await App.client.GetTable<dogBreed>().Where(breed => breed.id == dog.dogBreed_id).ToListAsync();


                        foreach (var b in breedName)
                        {
                            breed_Name = b.breedName;
                        }
                        double resultKilometers = Math.Round(getDistance(user_latitude, user_longtitude, otherUser_latitude, otherUser_longtitude), 2);
                        
                        List<SettingsData> checker = null;
                        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                        {
                            conn.CreateTable<SettingsData>();
                            checker = conn.Table<SettingsData>().ToList();
                            conn.Close();
                        };

                        if(resultKilometers < double.Parse(checker[0].breedingKilometers))
                        {
                            System.Uri url = new System.Uri(dog.dogImage);
                            _Doglist.Add(new dogInfo()
                            {
                                id = dog.id,
                                dogPurpose_id = dog.dogPurpose_id,
                                dogBreed_id = dog.dogBreed_id,
                                dogName = dog.dogName,
                                dogGender = dog.dogGender,
                                dogImage = dog.dogImage,
                                userid = resultKilometers.ToString() + "km"
                            });
                        } 
                        dogId.Add(dog.id);
                    }
                }
                
            }
            //pag may laman na either of the two tables (liked and disliked table)
            if (likedTable.Count != 0 || dislikedTable.Count != 0)
            {
                List<dogInfo> dogDisplay = new List<dogInfo>();
                List<likedDogs> lstOfLikedDogs = new List<likedDogs>();
                List<dislikedDogs> lstofDislikedDogs = new List<dislikedDogs>();
                List<string> lstOfNotIncludedDogs = new List<string>();
                List<string> ListOfDogs = new List<string>();


                var theLikedTable = await App.client.GetTable<likedDogs>().Where(x => x.userid == App.user_id).ToListAsync();
                var thedisLikedTable = await App.client.GetTable<dislikedDogs>().Where(x => x.userid == App.user_id).ToListAsync();
                

                if(theLikedTable.Count() == 0 & thedisLikedTable.Count() == 0)
                {
                    var dogs = await App.client.GetTable<dogInfo>().Where(x => x.userid != App.user_id && x.dogPurpose_id == "dk2emn1ik").ToListAsync();
                    foreach(var c in dogs)
                    {
                        dogDisplay.Add(c);
                    }
                }
                foreach (var row in theLikedTable)
                {
                    lstOfNotIncludedDogs.Add(row.dog_id);
                }
                foreach (var row in thedisLikedTable)
                {
                    lstOfNotIncludedDogs.Add(row.dog_id);
                }
                
                foreach (var dogs in lstOfNotIncludedDogs)
                {

                    var theDogInfoTable = await App.client.GetTable<dogInfo>().Where(x => x.id != dogs && x.userid != App.user_id).ToListAsync();
                    foreach (var doginfo in theDogInfoTable)
                    {
                        ListOfDogs.Add(doginfo.id);
                    }
                }
                List<string> finalListOfDogs = ListOfDogs.Distinct().ToList();

                if(likedTable.Count != 0)
                {
                    for (int x = 0; x < theLikedTable.Count(); x++)
                    {
                        if (finalListOfDogs.Contains(theLikedTable[x].dog_id))
                        {
                            finalListOfDogs.Remove(theLikedTable[x].dog_id);
                        }
                    }
                }

                if (dislikedTable.Count != 0)
                {
                    for (int x = 0; x < thedisLikedTable.Count(); x++)
                    {
                        if (finalListOfDogs.Contains(thedisLikedTable[x].dog_id))
                        {
                            finalListOfDogs.Remove(thedisLikedTable[x].dog_id);
                        }
                    }
                }

                foreach(var c in finalListOfDogs)
                {
                    var dogtable = await App .client.GetTable<dogInfo>().Where(x => x.id == c).ToListAsync();
                    foreach(var d in dogtable)
                    {
                        dogDisplay.Add(d);
                    }
                }
                displayAvaildogs(dogDisplay, finalListOfDogs);
            }
            UserDialogs.Instance.HideLoading();
        }

        public async void displayAvaildogs(List<dogInfo> dogDisplay, List<string> finalListOfDogs)
        {
            string breederName = "";
            foreach (var dog in dogDisplay)
            {
                var getLocation = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == dog.userid).ToListAsync();
                foreach (var c in getLocation)
                {
                    otherUser_latitude = double.Parse(c.latitude);
                    otherUser_longtitude = double.Parse(c.longtitude);
                }
                var breedName = await App.client.GetTable<dogBreed>().Where(breed => breed.id == dog.dogBreed_id).ToListAsync();

                foreach (var b in breedName)
                {
                    breederName = b.breedName;
                }

                double resultKilometers = Math.Round(getDistance(user_latitude, user_longtitude, otherUser_latitude, otherUser_longtitude), 2);

                List<SettingsData> checker = null;
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SettingsData>();
                    checker = conn.Table<SettingsData>().ToList();
                    conn.Close();
                };

                if (resultKilometers < double.Parse(checker[0].breedingKilometers))
                {
                    System.Uri url = new System.Uri(dog.dogImage);
                    _Doglist.Add(new dogInfo()
                    {
                        id = dog.id,
                        dogPurpose_id = dog.dogPurpose_id,
                        dogBreed_id = dog.dogBreed_id,
                        dogName = dog.dogName,
                        dogGender = dog.dogGender,
                        dogImage = dog.dogImage,
                        userid = resultKilometers.ToString() + "km"
                    });
                }
                
                dogId.Add(dog.id);
            }
            finalListOfDogs.Clear();
        }


        public ObservableCollection<dogInfo> DogList
        {
            get => _Doglist;
            set
            {
                _Doglist = value;
            }
        }

        protected override void OnAppearing()
        {
            App.uploadFlag = 1;
            breedingContentPage.Clear();
            breedingContentPage.Add(this);
            checkIfReady();
            initialLoad();
            UserDialogs.Instance.HideLoading();
            base.OnAppearing();

        }

        private async void checkIfReady()
        {
            UserDialogs.Instance.ShowLoading("Checking if the systeme is ready...");
            var checker = await App.client.GetTable<accountusers>().ToListAsync();
            var dogChecker = await App.client.GetTable<dogInfo>().ToListAsync();
            if (checker.Count < 5 || dogChecker.Count < 5)
            {
                App.loadingMessage = "Not enough users registered, the system is not yet ready";
                Application.Current.MainPage = new SystemNotReady();
                await Navigation.PushAsync(new SystemNotReady());
                await Navigation.PopToRootAsync();
            }
            else
            {
                return;
            }
        }

        private void btnNope_Clicked(object sender, EventArgs e)
        {
            try
            {
                int temp = pckrDogList.Items.Count();
                if (pckrDogList.SelectedIndex != - 1)
                {
                    try
                    {
                        if(pckrDogList.Items[pckrDogList.SelectedIndex] != "No dogs available")
                        {
                            swipeLeftAlgo();
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Please add a dog before using this breeding function", new TimeSpan(2));
                        }
                    }
                    catch (Exception)
                    {
                        UserDialogs.Instance.Toast("Please add a dog before using this breeding function", new TimeSpan(2));
                    }
                }
                else
                {
                    UserDialogs.Instance.Toast("Please add a dog before using this breeding function", new TimeSpan(2));
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {

                DisplayAlert("", "", "");
            }
        }

        private void btnLike_Clicked(object sender, EventArgs e)
        {
            try
            {
                int temp = pckrDogList.Items.Count();
                if (pckrDogList.SelectedIndex != -1)
                {
                    try
                    {
                        if (pckrDogList.Items[pckrDogList.SelectedIndex] != "No dogs available")
                        {
                            swipeRightAlgo();
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Please add a dog before using this breeding function", new TimeSpan(2));
                        }
                    }
                    catch (Exception)
                    {
                        UserDialogs.Instance.Toast("Please add a dog before using this breeding function", new TimeSpan(2));
                    }
                }
                else
                {
                    UserDialogs.Instance.Toast("Please add a dog before using this breeding function", new TimeSpan(2));
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {

                DisplayAlert("", "", "");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UploadDogPage());
        }

        public async void swipeLeftAlgo()
        {
            try
            {
                if (dogId.Count() == 0)
                {
                    await DisplayAlert("Sorry", "All available dogs was picked, please comeback tomorrow", "Okay");
                }
                else
                {
                    if (dogId.Count() != 0)
                    {
                        await swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left);
                        dislikedDogs dis_like = new dislikedDogs()
                        {
                            id = Guid.NewGuid().ToString("N").Substring(0, 10),
                            userid = App.user_id,
                            dog_id = dogId[0]
                        };
                        dogId.RemoveAt(0);
                        //temporary++;
                        await App.client.GetTable<dislikedDogs>().InsertAsync(dis_like);
                        Acr.UserDialogs.UserDialogs.Instance.Toast("You disliked the dog", new TimeSpan(2));
                        if (_Doglist.Count == 0)
                        {
                            loadDogs();
                        }
                    }
                    if (dogId.Count() == 0)
                    {
                        await DisplayAlert("Sorry", "All available dogs was picked, please comeback tomorrow", "Okay");
                    }
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
                if (dogId.Count() == 0)
                {
                    await DisplayAlert("Sorry", "All available dogs was picked, please comeback tomorrow", "Okay");
                }
                else
                {
                    if (dogId.Count() != 0)
                    {
                        await swipeView.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
                        likedDogs like = new likedDogs()
                        {
                            id = Guid.NewGuid().ToString("N").Substring(0, 11),
                            userid = App.user_id,
                            dog_id = dogId[0]
                        };     
                        
                        await App.client.GetTable<likedDogs>().InsertAsync(like);
                        

                        Acr.UserDialogs.UserDialogs.Instance.Toast("You liked the dog", new TimeSpan(2));
                        if (dogId.Count() == 0)
                        {
                            loadDogs();
                        }
                    }
                    if (dogId.Count() == 0)
                    {
                        await DisplayAlert("Sorry", "All available dogs was picked, please comeback tomorrow", "Okay");
                    }

                }
                MatchedChecker();
                dogId.RemoveAt(0);

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

        private void pckrDogList_SelectedIndexChanged(object sender, EventArgs e)
        {
            initialLoad();
        }

        private async void loadRelatedDogs()
        {
            _Doglist.Clear();
            dogId.Clear();
            if(_Doglist.Count() == 0)
            {
                int index = _mydoglist.FindIndex(a => a.dogName == pckrDogList.Items[pckrDogList.SelectedIndex]);
                var availDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid != App.user_id && x.dogBreed_id == _breedIdList[index] && x.dogGender != _mydoglist[index].dogGender).ToListAsync();
                foreach(var dog in availDogs)
                {
                    //dae pa nafifiter ang dislike
                    var finalDogs = await App.client.GetTable<likedDogs>().Where(x => x.dog_id == dog.id && x.userid == App.user_id).ToListAsync();
                    if(finalDogs.Count == 0)
                    {
                        foreach (var d in availDogs)
                        {
                            double resultKilometers = Math.Round(getDistance(user_latitude, user_longtitude, otherUser_latitude, otherUser_longtitude), 2);

                            List<SettingsData> checker = null;
                            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                            {
                                conn.CreateTable<SettingsData>();
                                checker = conn.Table<SettingsData>().ToList();
                                conn.Close();
                            };

                            if (resultKilometers < double.Parse(checker[0].breedingKilometers))
                            {
                                _Doglist.Add(new dogInfo()
                                {
                                    id = dog.id,
                                    dogPurpose_id = dog.dogPurpose_id,
                                    dogBreed_id = dog.dogBreed_id,
                                    dogName = dog.dogName,
                                    dogGender = dog.dogGender,
                                    dogImage = dog.dogImage,
                                    userid = resultKilometers.ToString() + "km"
                                });
                                dogId.Add(dog.id);
                            }
                        }
                    }
                }
            }
        }

        private void pckrMatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            initialLoad();
        }

        private void initialLoad()
        {
            if (pckrDogList.SelectedIndex == -1 && pckrMatchType.SelectedIndex == 0)
            {
                loadDogs();
            }

            if (pckrDogList.SelectedIndex != -1 && pckrMatchType.SelectedIndex == 1)
            {
                loadRelatedDogs();
            }

            if (pckrDogList.SelectedIndex != -1 && pckrMatchType.SelectedIndex == 0)
            {
                loadDogs();
            }
        }

        private void SwipeGestureRecognizer_Swiped_1(object sender, SwipedEventArgs e)
        {
            //right
            swipeRightAlgo();
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
        public async void MatchedChecker()
        {
            if(dogId.Count() != 0)
            {
                List<dogInfo> mylistofDogs = new List<dogInfo>();
                List<likedDogs> MatchedDogs = new List<likedDogs>();
                string liked_DogId = dogId[0];

                var ownerOfDog = await App.client.GetTable<dogInfo>().Where(x => x.id == liked_DogId).ToListAsync();
                var matchedDogs = await App.client.GetTable<likedDogs>().Where(x => x.userid == ownerOfDog[0].userid).ToListAsync();

                if(matchedDogs.Count != 0)
                {
                    int index = pckrID.FindIndex(a => a.dogName == pckrDogList.Items[pckrDogList.SelectedIndex]);
                    string dog1 = pckrID[index].id;

                    dogMatches match = new dogMatches()
                    {
                        id = Guid.NewGuid().ToString("N").Substring(0, 20),
                        dog1 = dog1,
                        dog2 = liked_DogId,
                        markAsDone = "False"
                    };

                    try
                    {
                        await App.client.GetTable<dogMatches>().InsertAsync(match);

                        await DisplayAlert("Its a match!", "Youve been match with someone else", "Okay");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

    }
}