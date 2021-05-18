using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
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
    public partial class RelatedShopsPage : ContentPage
    {
        public ObservableCollection<dogRelatedEstablishments> _listOfEstablishments = new ObservableCollection<dogRelatedEstablishments>();
        public RelatedShopsPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadPlaces();
        }

        private async void LoadPlaces()
        {
            var establishments = await App.client.GetTable<dogRelatedEstablishments>().ToListAsync();
            foreach(var c in establishments)
            {
                
            }
        }

        private void addShop_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddShop());
        }

        private void btnSeeAllEstablishments_Clicked(object sender, EventArgs e)
        {

        }
    }
}