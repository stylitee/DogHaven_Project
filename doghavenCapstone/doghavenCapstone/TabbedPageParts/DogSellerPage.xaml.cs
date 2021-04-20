using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.TabbedPageParts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogSellerPage : ContentPage
    {
        public ObservableCollection<dogSeller> _dogSellers = new ObservableCollection<dogSeller>();
        public static List<ContentPage> _DogSellerPage = new List<ContentPage>();
        public DogSellerPage()
        {
            InitializeComponent();
            BindingContext = this;
            checkifRegistered();
            lostSellerTypes();
            _DogSellerPage.Add(this);
        }

        public void lostSellerTypes()
        {
            pickersType.Items.Add("PCCI registred sellers");
            pickersType.Items.Add("Non-PCCI registered sellers");
            pickersType.SelectedIndex = 0;
        }

        public async void checkifRegistered()
        {
            var accountChecker = await App.client.GetTable<dogSeller>().Where(x => x.userid == App.user_id).ToListAsync();
            
            if(accountChecker.Count == 0)
            {
                await DisplayAlert("Prompt", "Are you a dog seller? You can change your user type in the profile and change it to Seller", "Okay"); ;
                //sqldatabase that will save the choice of the user so that it wont prompt everytime it shows this page
                return;
            }
            else
            {
                return;
            }
            
        }

        public ObservableCollection<dogSeller> dogSellers
        {
            get => _dogSellers;
            set
            {
                _dogSellers = value;
            }
        }

        public async void loadSellers()
        {
            _dogSellers.Clear();
            List<string> breed_ids = new List<string>();
            List<string> breedListNames = new List<string>();
            int numberOfDogs = 0;
            breed_ids.Clear();
            breedListNames.Clear();
            if (VariableStorage.isRegistred == "Yes")
            {
                var tableSeller = await App.client.GetTable<dogSeller>().Where(x => x.userid != App.user_id && x.isRegistered == "Yes").ToListAsync();
                foreach (var seller in tableSeller)
                {
                    
                    string _fullName = "", _sellerImage = "", breedsOwned = "";
                    
                    var userInfo = await App.client.GetTable<accountusers>().Where(x => x.id == seller.userid).ToListAsync();
                    var dogInfo = await App.client.GetTable<dogInfo>().Where(x => x.userid == seller.userid).ToListAsync();
                    foreach(var info in userInfo)
                    {
                        _fullName = info.fullName;
                        _sellerImage = info.userImage;
                    }
                    foreach(var info in dogInfo)
                    {
                        numberOfDogs++;
                        breed_ids.Add(info.dogBreed_id);
                    }
                    if(breed_ids.Count <= 1)
                    {
                        foreach(var dogs in breed_ids)
                        {
                            var getBreed = await App.client.GetTable<dogBreed>().Where(x => x.id == dogs).ToListAsync();
                            foreach(var c in getBreed)
                            {
                                breedsOwned = breedsOwned + c.breedName;
                            }
                            
                        }
                    }
                    if(breed_ids.Count > 1)
                    {
                        foreach (var dogs in breed_ids)
                        {
                            var getBreed = await App.client.GetTable<dogBreed>().Where(x => x.id == dogs).ToListAsync();
                            foreach (var c in getBreed)
                            {
                                breedsOwned = breedsOwned + ", " + dogs;
                            }

                        }
                    }
                    _dogSellers.Add(new dogSeller()
                    {
                        id = seller.id,
                        userid = seller.userid,
                        fullName = "Seller Name: " + _fullName,
                        dogsOwnedForSelling = "Number of Dogs: " + numberOfDogs.ToString(),
                        breedsName = "Breed(s) owned: " + breedsOwned,
                        sellerImage = _sellerImage
                    });
                }
            } 
        }

        public async void loadUnregistered()
        {
            _dogSellers.Clear();
            List<string> lstOfBreedsOwned = new List<string>();
            int numberOfDogs = 0;
            lstOfBreedsOwned.Clear();
            var tableSeller = await App.client.GetTable<dogSeller>().Where(x => x.userid != App.user_id && x.isRegistered == "No").ToListAsync();
            foreach (var seller in tableSeller)
            {
                string _fullName = "", _sellerImage = "", breedsOwned = "";

                var userInfo = await App.client.GetTable<accountusers>().Where(x => x.id == seller.userid).ToListAsync();
                var dogInfo = await App.client.GetTable<dogInfo>().Where(x => x.userid == seller.userid).ToListAsync();
                foreach (var info in userInfo)
                {
                    _fullName = info.fullName;
                    _sellerImage = info.userImage;
                }
                foreach (var info in dogInfo)
                {
                    numberOfDogs++;
                    lstOfBreedsOwned.Add(info.breed_Name);
                }
                if (lstOfBreedsOwned.Count <= 1)
                {
                    foreach (var dogs in lstOfBreedsOwned)
                    {
                        breedsOwned = breedsOwned + dogs;
                    }
                }
                if (lstOfBreedsOwned.Count > 1)
                {
                    foreach (var dogs in lstOfBreedsOwned)
                    {
                        breedsOwned = breedsOwned + ", " + dogs;
                    }
                }
                _dogSellers.Add(new dogSeller()
                {
                    id = seller.id,
                    userid = seller.userid,
                    fullName = "Seller Name: " + _fullName,
                    dogsOwnedForSelling = "Number of Dogs: " + numberOfDogs.ToString(),
                    breedsName = "Breed(s) owned: " + breedsOwned,
                    sellerImage = _sellerImage
                });
            }
        }

        private void pickersType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pickersType.SelectedItem.ToString() == "PCCI registred sellers")
            {
                VariableStorage.isRegistred = "Yes";
                loadSellers();
            }
            if(pickersType.SelectedItem.ToString() == "Non-PCCI registered sellers")
            {
                VariableStorage.isRegistred = "No";
                loadUnregistered();
            }
        }

        private void addLostDog_Clicked(object sender, EventArgs e)
        {

        }
    }
}