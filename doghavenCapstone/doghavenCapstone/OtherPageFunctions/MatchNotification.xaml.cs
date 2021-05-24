using doghavenCapstone.MainPages;
using doghavenCapstone.Model;
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
    public partial class MatchNotification : ContentPage
    {
        public static string user1 = "", user2 = "";
        public MatchNotification()
        {
            InitializeComponent();
            user1 = BreedMatchingPage.userowner1;
            user2 = BreedMatchingPage.userowner2;
            loadInformation();
        }

        private async void loadInformation()
        {
            var info = await App.client.GetTable<accountusers>().Where(x => x.id == user2).ToListAsync();
            lblTitle.Text = "You and " + info[0].fullName + "'s Dog is match!";
        }

        private void btnChat_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MessagesPage());
        }

        private void btnSwipe_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}