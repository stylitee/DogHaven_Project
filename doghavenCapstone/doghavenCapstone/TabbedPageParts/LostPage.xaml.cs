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
    public partial class LostPage : ContentPage
    {
        public ObservableCollection<LostDogs> _LostDoglist = new ObservableCollection<LostDogs>();
        public LostPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public async void LoadLostDogs()
        {
            var LostList = await App.client.GetTable<LostDogs>().ToListAsync();

            foreach(var c in LostList)
            {
                //displaying of data in list view
            }
        }

        public ObservableCollection<LostDogs> LostDoglist
        {
            get => _LostDoglist;
            set
            {
                _LostDoglist = value;
            }
        }
    }
}