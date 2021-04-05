using doghavenCapstone.InitialPages;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone
{
    public partial class App : Application
    {
        public static MobileServiceClient client = new MobileServiceClient("https://doghaven-serverapp.azurewebsites.net");
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
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
