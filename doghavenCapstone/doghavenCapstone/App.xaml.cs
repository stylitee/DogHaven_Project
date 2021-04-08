using doghavenCapstone.InitialPages;
using doghavenCapstone.OtherPageFunctions;
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
        public static string user_id = "";
        public App(string databaseLocation)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new UploadDogPage();
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
