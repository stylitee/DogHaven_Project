using doghavenCapstone.ClassHelper;
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

namespace doghavenCapstone.DetailsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class dogForSaleDetails : ContentPage
    {
        public dogForSaleDetails()
        {
            InitializeComponent();
            loadDogInformation();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private async void loadDogInformation()
        {
            var dogInformation = await App.client.GetTable<dogInfo>().Where(x => x.id == VariableStorage.dogDetails_doginfoID).ToListAsync();
            var sellerinformation = await App.client.GetTable<dogSeller>().Where(x => x.id == VariableStorage.dogDetails_sellerid).ToListAsync();
            var userinformation = await App.client.GetTable<accountusers>().Where(x => x.id == sellerinformation[0].userid).ToListAsync();
            imgDog.Source = dogInformation[0].dogImage;
            lblBreed.Text = "Breed: " + VariableStorage.dogDetails_breeedName;
            lblAge.Text = "Age: " + VariableStorage.dogDetails_age;
            lblPrice.Text = "Price: " + VariableStorage.dogDetails_price;
            lblVaccinated.Text = "Vaccinated: " + VariableStorage.dogDetails_vaccine;
            lblCompletePapers.Text = "Complete Papers: " + VariableStorage.dogDetails_completepapers;
            lblOwner.Text = "Owner: " + userinformation[0].fullName;
        }
    }
}