using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketPlacePage : ContentPage
    {
        //public ObservableCollection<>
        public MarketPlacePage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        
    }
}