using doghavenCapstone.ClassHelper;
using doghavenCapstone.MainPages;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogInformationPage : ContentPage
    {
        public ObservableCollection<dogInfo> _Doglist = new ObservableCollection<dogInfo>();
        public DogInformationPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            loadDogInformation();
            BindingContext = this;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void loadDogInformation()
        {
            
            string ownerName = "";
            var doginfo = await App.client.GetTable<dogInfo>().Where(x => x.id == App.dog_id).ToListAsync();
            var accountInfo = await App.client.GetTable<accountusers>().Where(x => x.id == doginfo[0].userid).ToListAsync();
            imgDogImage.Source = doginfo[0].dogImage;
            lblNameOfTheDog.Text = doginfo[0].dogName;
            foreach (var person in accountInfo)
            {
                ownerName = person.fullName;
            }
            var getDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == accountInfo[0].id).ToListAsync();
            lblOwner.Text = "Owned by: " + ownerName;

            var allDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == accountInfo[0].id).ToListAsync();
            _Doglist.Clear();
            foreach (var c in allDogs)
            {
                /*if()*/
                _Doglist.Add(new dogInfo()
                {
                    dogImage = c.dogImage
                });
            }
        }

        protected override void OnAppearing()
        {
            App.uploadFlag = 2;
            base.OnAppearing();

        }
        public ObservableCollection<dogInfo> DogList
        {
            get => _Doglist;
            set
            {
                _Doglist = value;
            }
        }
    }
}