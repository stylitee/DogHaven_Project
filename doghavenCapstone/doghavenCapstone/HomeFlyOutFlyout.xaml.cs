using doghavenCapstone.MainPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeFlyOutFlyout : ContentPage
    {
        public ListView ListView;

        public HomeFlyOutFlyout()
        {
            InitializeComponent();

            BindingContext = new HomeFlyOutFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class HomeFlyOutFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomeFlyOutFlyoutMenuItem> MenuItems { get; set; }

            public HomeFlyOutFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<HomeFlyOutFlyoutMenuItem>(new[]
                {
                    new HomeFlyOutFlyoutMenuItem { Id = 0, Title = "Profile", Icon = "profile.png"},
                    new HomeFlyOutFlyoutMenuItem { Id = 1, Title = "Breed Matching" , Icon = "Breeding.png", TargetType = typeof(BreedMatchingPage) },
                    new HomeFlyOutFlyoutMenuItem { Id = 2, Title = "Marketplace", Icon = "MarketPlace.png" },
                    new HomeFlyOutFlyoutMenuItem { Id = 3, Title = "Adoption", Icon = "adoption.png" },
                    new HomeFlyOutFlyoutMenuItem { Id = 4, Title = "Lost and Found", Icon = "Lost_and_Found.png" },
                    new HomeFlyOutFlyoutMenuItem { Id = 5, Title = "Settings", Icon = "settings.png" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}