using doghavenCapstone.InitialPages;
using doghavenCapstone.PreventerPage;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone
{
    public partial class App : Application
    {
        public static MobileServiceClient client = new MobileServiceClient("https://myserver-doghaven.azurewebsites.net");
        public static string DatabaseLocation = string.Empty;
        public static string loadingMessage = "";
        public App(string databaseLocation)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new Loading();
            DatabaseLocation = databaseLocation;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
