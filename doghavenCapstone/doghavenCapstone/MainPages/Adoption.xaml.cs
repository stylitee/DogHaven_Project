using doghavenCapstone.OtherPageFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Adoption : ContentPage
    {
        public Adoption()
        {
            InitializeComponent();
        }

        private void btnOpenMap_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserCurrentLocation());
        }
    }
}