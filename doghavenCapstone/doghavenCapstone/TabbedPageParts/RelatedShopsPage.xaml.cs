using doghavenCapstone.OtherPageFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.TabbedPageParts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelatedShopsPage : ContentPage
    {
        public RelatedShopsPage()
        {
            InitializeComponent();
        }

        private void addShop_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddShop());
        }
    }
}