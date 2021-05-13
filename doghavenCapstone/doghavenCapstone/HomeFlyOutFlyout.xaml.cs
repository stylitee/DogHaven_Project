using doghavenCapstone.ClassHelper;
using doghavenCapstone.MainPages;
using Plugin.LocalNotification;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
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
            NotificationChecker();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            NotificationCenter.Current.NotificationReceived += Current_NotificationReceived;
            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;
            BindingContext = new HomeFlyOutFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private void Current_NotificationTapped(NotificationTappedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Match Confirmation", "Yove got a match with someone else", "Okay");
            });
        }

        private void Current_NotificationReceived(NotificationReceivedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Match Confirmation", "Yove got a match with someone else", "Okay");
            });
        }

        public void NotificationChecker()
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                AppHelpers.PushNotificationInit();
                return true;
            });
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        class HomeFlyOutFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomeFlyOutFlyoutMenuItem> MenuItems { get; set; }

            public HomeFlyOutFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<HomeFlyOutFlyoutMenuItem>(new[]
                {
                    new HomeFlyOutFlyoutMenuItem { Id = 0, Title = "Profile", Icon = "profile.png", TargetType = typeof(ProfilePage)},
                    new HomeFlyOutFlyoutMenuItem { Id = 1, Title = "Breed Matching" , Icon = "Breeding.png", TargetType = typeof(BreedMatchingPage) },
                    new HomeFlyOutFlyoutMenuItem { Id = 2, Title = "Marketplace", Icon = "MarketPlace.png",TargetType = typeof(MarketPlacePage) },
                    new HomeFlyOutFlyoutMenuItem { Id = 3, Title = "Adoption", Icon = "adoption.png", TargetType = typeof(Adoption) },
                    new HomeFlyOutFlyoutMenuItem { Id = 4, Title = "Lost and Found", Icon = "Lost_and_Found.png" , TargetType = typeof(LostAndFoundHome)},
                    new HomeFlyOutFlyoutMenuItem { Id = 5, Title = "Messages", Icon = "settings.png", TargetType = typeof(SettingsPage) },
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