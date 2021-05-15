using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.InitialPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroPage : ContentPage
    {
        public IntroPage()
        {
            InitializeComponent();
            var assembly = typeof(IntroPage);
            imgLogo.Source = ImageSource.FromResource("doghavenCapstone.Assets.introPageLogo.png", assembly);
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5000);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}