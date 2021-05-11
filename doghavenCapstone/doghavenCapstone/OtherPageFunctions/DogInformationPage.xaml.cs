using doghavenCapstone.ClassHelper;
using doghavenCapstone.MainPages;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
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
        public DogInformationPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            loadDogInformation();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void loadDogInformation()
        {
            string ownerName = "";
            var getName = await App.client.GetTable<accountusers>().Where(x => x.id == App.dog_userID).ToListAsync();
            imgDogImage.Source = App.dog_image;
            lblNameOfTheDog.Text = App.dog_name;
            foreach(var person in getName)
            {
                ownerName = person.fullName;
            }
            var getDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.dog_userID).ToListAsync();
            lblOwner.Text = "Owned by: " + ownerName;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}