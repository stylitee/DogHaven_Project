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

namespace doghavenCapstone.DetailsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerDetails : ContentPage
    {
        public ObservableCollection<DogPrice> _dogPrices = new ObservableCollection<DogPrice>();
        List<DogPrice> dogPriceInformation = new List<DogPrice>();
        List<dogInfo> doginformationList = new List<dogInfo>();
        List<dogSeller> sellerInformations = new List<dogSeller>();
        public static List<ContentPage> dogForsale = new List<ContentPage>();
        public static string breed__id = "";
        List<string> ListOfBreeds = new List<string>();
        
        public SellerDetails()
        {
            InitializeComponent();
            dogForsale.Add(this);
            BindingContext = this;
            LoadBreeds();
            loadUserInfo();
        }

        public async void LoadBreeds()
        {
            _dogPrices.Clear();
            pckrBreedsOwned.Items.Clear();
            dogPriceInformation.Clear();
            doginformationList.Clear();
            ListOfBreeds.Clear();
            var sellerInfo = await App.client.GetTable<dogSeller>().Where(x => x.userid == VariableStorage.sellersUser_id).ToListAsync();
            foreach (var info in sellerInfo)
            {
                sellerInformations.Add(info);
            }

            var dogInformation = await App.client.GetTable<DogPrice>().Where(x => x.seller_id == sellerInformations[0].id).ToListAsync();
            foreach(var c in dogInformation)
            {
                dogPriceInformation.Add(c);
            }

            foreach (var info in dogPriceInformation)
            {
                var getInfo = await App.client.GetTable<dogInfo>().Where(x => x.id == info.doginfo_id ).ToListAsync();
                foreach (var breeds in getInfo)
                {
                    doginformationList.Add(breeds);
                }
            }
            pckrBreedsOwned.Items.Add("All");
            foreach(var breeds in doginformationList)
            {
                var getInfo = await App.client.GetTable<dogBreed>().Where(x => x.id == breeds.dogBreed_id).ToListAsync();
                foreach(var info in getInfo)
                {
                    pckrBreedsOwned.Items.Add(info.breedName);
                }
            }

            pckrBreedsOwned.SelectedIndex = 0;
        }
        protected override void OnAppearing()
        {
            App.uploadFlag = 1;
            base.OnAppearing();
        }

        public ObservableCollection<DogPrice> dogPrices
        {
            get => _dogPrices;
            set
            {
                _dogPrices = value;
            }
        }

        public async void loadSelectedDog(string breedNames)
        {
            _dogPrices.Clear();
            pckrBreedsOwned.Items.Clear();
            dogPriceInformation.Clear();
            doginformationList.Clear();
            ListOfBreeds.Clear();

            var dogInformation = await App.client.GetTable<DogPrice>().Where(x => x.seller_id == VariableStorage.sellersUser_id).ToListAsync();
            foreach(var info in dogInformation)
            {
                var doginfos = await App.client.GetTable<dogInfo>().Where(x => x.id == info.doginfo_id).ToListAsync();
                foreach (var getimage in doginfos)
                {
                    var _breedName = await App.client.GetTable<dogBreed>().Where(x => x.id == getimage.dogBreed_id).ToListAsync();
                    if(breedNames == _breedName[0].breedName)
                    {
                        _dogPrices.Add(new DogPrice
                        {
                            dogImage = getimage.dogImage,
                            withCompletePapers = "Complete Papers: " + info.withCompletePapers,
                            completeVaccines = "Comeplete Vacination: " + info.completeVaccines,
                            price = "Price: " + info.price
                        });
                    }
                    else if(breedNames == "All")
                    {
                        _dogPrices.Add(new DogPrice
                        {
                            dogImage = getimage.dogImage,
                            withCompletePapers = "Complete Papers: " + info.withCompletePapers,
                            completeVaccines = "Comeplete Vacination: " + info.completeVaccines,
                            price = "Price: " + info.price
                        });
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        public async void loadUserInfo()
        {
            string _userImage = "", _fullName = "", _address_id = "", _complete_address = "";
            var userInfo = await App.client.GetTable<accountusers>().Where(x => x.id == VariableStorage.sellersUser_id).ToListAsync();
            foreach (var info in userInfo)
            {
                _userImage = info.userImage;
                _fullName = info.fullName;
                _address_id = info.address_id;
            }
            var userAddress = await App.client.GetTable<usersaddress>().Where(x => x.id == _address_id).ToListAsync();
            foreach (var info in userAddress)
            {
                _complete_address = info.streetname + ", " + info.barangay;
            }

            lblName.Text = "Name: " + _fullName;
            lblAddress.Text = "Address: " + _complete_address;
            if(VariableStorage.isRegistred == "Yes")
            {
                lblLicense.Text = "PCCI Registered: " + VariableStorage.isRegistred;
            }
            else
            {
                lblLicense.Text = "PCCI Registered: No";
            }

            imgOwnerImage.Source = _userImage;

        }

        private void pckrBreedsOwned_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pckrBreedsOwned.Items[pckrBreedsOwned.SelectedIndex].ToString() == "All")
            {
                loadAllDogs();
            }
            else
            {
                loadSelectedDog(pckrBreedsOwned.Items[pckrBreedsOwned.SelectedIndex].ToString());
            }
        }

        private async void loadAllDogs()
        {
            _dogPrices.Clear();
            foreach(var info in dogPriceInformation)
            {
                string image = "", breed_id = "", breed_name = "";
                var dogdetails = await App.client.GetTable<dogInfo>().Where(x => x.id == info.doginfo_id).ToListAsync();
                foreach(var c in dogdetails)
                {
                    image = c.dogImage;
                    breed_id = c.dogBreed_id;
                }
                var dogBreed = await App.client.GetTable<dogBreed>().Where(x => x.id == breed_id).ToListAsync();
                foreach(var g in dogBreed)
                {
                    breed_name = g.breedName;
                }
                _dogPrices.Add(new DogPrice()
                {
                    id = info.id,
                    doginfo_id = info.doginfo_id,
                    price = "Price: " + info.price,
                    withCompletePapers = info.withCompletePapers,
                    completeVaccines = info.completeVaccines,
                    Age = "Age: " + info.Age,
                    seller_id = info.seller_id,
                    dogImage = image,
                    dogBreed = "Breed: " + breed_name
                });
            }

            dogPriceInformation.Clear();

        }
    }
}