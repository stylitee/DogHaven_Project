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
        List<string> getDogID = new List<string>();
        List<string> getBreedID = new List<string>();
        List<string> ListOfBreeds = new List<string>();
        public SellerDetails()
        {
            InitializeComponent();
            BindingContext = this;
            LoadBreeds();
            loadUserInfo();
        }

        public async void LoadBreeds()
        {
            pckrBreedsOwned.Items.Clear();
            getDogID.Clear();
            getBreedID.Clear();
            ListOfBreeds.Clear();
            var dogInformation = await App.client.GetTable<DogPrice>().Where(x => x.seller_id == VariableStorage.sellersUser_id).ToListAsync();
            foreach (var info in dogInformation)
            {
                getDogID.Add(info.doginfo_id);
            }

            foreach (var id in getDogID)
            {
                var getInfo = await App.client.GetTable<dogInfo>().Where(x => x.id == id).ToListAsync();
                foreach (var breeds in getInfo)
                {
                    getBreedID.Add(breeds.dogBreed_id);
                }
            }
            pckrBreedsOwned.Items.Add("All");
            foreach(var breeds in getBreedID)
            {
                var getInfo = await App.client.GetTable<dogInfo>().Where(x => x.id == breeds).ToListAsync();
                foreach(var info in getInfo)
                {
                    pckrBreedsOwned.Items.Add(info.breed_Name);
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
            getBreedID.Clear();
            ListOfBreeds.Clear();
            getBreedID.Clear();
            
            var dogInformation = await App.client.GetTable<DogPrice>().Where(x => x.seller_id == VariableStorage.sellersUser_id).ToListAsync();
            foreach(var info in dogInformation)
            {
                var doginfos = await App.client.GetTable<dogInfo>().Where(x => x.id == info.doginfo_id).ToListAsync();
                foreach (var getimage in doginfos)
                {
                    if(breedNames == getimage.breed_Name)
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

        }

        private void pckrBreedsOwned_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSelectedDog(pckrBreedsOwned.SelectedItem.ToString());
        }
    }
}