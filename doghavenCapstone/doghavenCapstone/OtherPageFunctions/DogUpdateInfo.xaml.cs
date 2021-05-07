using doghavenCapstone.MainPages;
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
    public partial class DogUpdateInfo : ContentPage
    {
        public DogUpdateInfo()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            App.doginfo_flag = 0;
            base.OnAppearing();

        }
    }
}