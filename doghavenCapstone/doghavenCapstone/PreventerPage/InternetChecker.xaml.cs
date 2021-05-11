using doghavenCapstone.ClassHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.PreventerPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetChecker : ContentPage
    {
        public InternetChecker()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            var assembly = typeof(InternetChecker);
            imgNoConnection.Source = ImageSource.FromResource("doghavenCapstone.Assets.satelite.png", assembly);
           
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess.ToString() == "Internet")
            {
                Navigation.PopAsync();
            }
        }
    }
}