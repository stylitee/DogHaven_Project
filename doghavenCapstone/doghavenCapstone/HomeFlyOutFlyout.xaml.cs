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
                    new HomeFlyOutFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new HomeFlyOutFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new HomeFlyOutFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new HomeFlyOutFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new HomeFlyOutFlyoutMenuItem { Id = 4, Title = "Page 5" },
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