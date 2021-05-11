using doghavenCapstone.PreventerPage;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace doghavenCapstone.ClassHelper
{
    public class AppHelpers
    {
        public static void checkConnection(ContentPage mainpage, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess.ToString() != "Internet")
            {
                mainpage.Navigation.PushAsync(new InternetChecker());
            }
        }
    }
}
