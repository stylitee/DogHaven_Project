using Acr.UserDialogs;
using doghavenCapstone.LocalDBModel;
using doghavenCapstone.OtherPageFunctions;
using SQLite;
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
            loadSqlConfiguration();
        }

        private void loadSqlConfiguration()
        {
            List<accountsLoggedIn> checker = null;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<accountsLoggedIn>();
                checker = conn.Table<accountsLoggedIn>().Where(x => x.userid == App.user_id).ToList();
                conn.Close();
            };

            foreach (var c in checker)
            {
                accountsLoggedIn account = new accountsLoggedIn()
                {
                    id = c.id,
                    userid = c.userid,
                    fullName = c.fullName,
                    username = c.username,
                    userPassword = c.userPassword,
                    isLoggedIn = "No"
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<accountsLoggedIn>();
                    conn.Update(account);
                    conn.Close();
                };
            }
        }

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            Phone.CloseApplication();
        }
    }
}