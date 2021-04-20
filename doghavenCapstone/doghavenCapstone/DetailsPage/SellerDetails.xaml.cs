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
        public ObservableCollection<DogPrices> _dogPrices = new ObservableCollection<DogPrices>();
        public SellerDetails()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public ObservableCollection<DogPrices> dogPrices
        {
            get => _dogPrices;
            set
            {
                _dogPrices = value;
            }
        }

        public async void loadSelectedDog()
        {
            List<DogPrices> lstOfDogs = new List<DogPrices>();
            lstOfDogs.Clear();
            string _userImage = "", _fullName = "", _address_id = "", _complete_address = "";
            var userInfo = await App.client.GetTable<accountusers>().Where(x => x.id == VariableStorage.sellersUser_id).ToListAsync();
            foreach(var info in userInfo)
            {
                _userImage = info.userImage;
                _fullName = info.fullName;
                _address_id = info.address_id;
            }
            var userAddress = await App.client.GetTable<usersaddress>().Where(x => x.id == _address_id).ToListAsync();
            foreach(var info in userAddress)
            {
                _complete_address = info.streetname + ", " + info.barangay;
            }
            var dogInformation = await App.client.GetTable<DogPrices>().Where(x => x.seller_id == VariableStorage.sellersUser_id).ToListAsync();
            foreach(var info in dogInformation)
            {
                lstOfDogs.Add(info);
            }
        }
    }
}