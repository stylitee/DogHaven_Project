using Acr.UserDialogs;
using doghavenCapstone.OtherPageFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.PreventerPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SystemNotReady : ContentPage
    {
        public SystemNotReady()
        {
            InitializeComponent();

            var assembly = typeof(SystemNotReady);
            imgCuteDog.Source = ImageSource.FromResource("doghavenCapstone.Assets.dog_questioning.gif", assembly);
            lblMessage.Text = App.loadingMessage;
            UserDialogs.Instance.HideLoading();
        }

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Phone.CloseApplication();
        }
    }
}