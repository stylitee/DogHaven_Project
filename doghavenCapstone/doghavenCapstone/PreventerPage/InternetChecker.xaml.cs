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
            var assembly = typeof(InternetChecker);
            imgNoConnection.Source = ImageSource.FromResource("doghavenCapstone.Assets.satelite.png", assembly);
           
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}