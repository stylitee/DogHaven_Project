using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLostDogPage : ContentPage
    {
        public AddLostDogPage()
        {
            InitializeComponent();
        }

        private void btnOpenMaps_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PinLostDogPage());
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {

        }
    }
}