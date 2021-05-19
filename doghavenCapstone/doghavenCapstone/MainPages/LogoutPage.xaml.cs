using doghavenCapstone.ClassHelper;
using doghavenCapstone.InitialPages;
using doghavenCapstone.LocalDBModel;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutPage : ContentPage
    {
        public LogoutPage()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AppHelpers.checkConnection(this, e);
        }

        private void btnConfirm_Clicked(object sender, EventArgs e)
        {
            List<accountsLoggedIn> checker = null;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<accountsLoggedIn>();
                checker = conn.Table<accountsLoggedIn>().Where(x => x.userid == App.user_id).ToList();
                conn.Close();
            };

            foreach(var c in checker)
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
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());
        }
    }
}