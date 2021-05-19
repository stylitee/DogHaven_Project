using Acr.UserDialogs;
using doghavenCapstone.LocalDBModel;
using doghavenCapstone.Model;
using doghavenCapstone.OtherPageFunctions;
using SQLite;
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
            try
            {
                List<accountsLoggedIn> checker = null;
                List<accountsLoggedIn> tableChecker = null;
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<accountsLoggedIn>();
                    tableChecker = conn.Table<accountsLoggedIn>().ToList();
                    checker = conn.Table<accountsLoggedIn>().Where(x => x.isLoggedIn == "Yes").ToList();
                    conn.Close();
                };

                if(tableChecker.Count != 0)
                {
                    if (checker.Count != 0)
                    {
                        UserDialogs.Instance.ShowLoading("Please wait while we logged you in");
                        App.user_id = checker[0].userid;
                        var newAccountChecker = await App.client.GetTable<dogInfo>().ToListAsync();
                        var dogchecker = await App.client.GetTable<dogInfo>().ToListAsync();
                        var userDogs = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id).ToListAsync();
                        var location = await App.client.GetTable<getCurrentLocation>().Where(x => x.user_id == App.user_id).ToListAsync();

                        if (newAccountChecker.Count + 1 <= 5 && dogchecker.Count <= 5 && location.Count == 0)
                        {
                            await Navigation.PushAsync(new GetUsersLocation());
                        }
                        if (newAccountChecker.Count + 1 <= 5 && dogchecker.Count <= 5 && location.Count != 0)
                        {
                            if (userDogs.Count != 0)
                            {
                                await Navigation.PushAsync(new UploadDogPage());
                            }
                            else
                            {
                                await Navigation.PushAsync(new NewAccountVerify());
                            }

                        }
                        if (newAccountChecker.Count() + 1 >= 5 && dogchecker.Count >= 5 && location.Count != 0)
                        {
                            Application.Current.MainPage = new NavigationPage(new HomeFlyOut());
                            UserDialogs.Instance.HideLoading();
                        }
                        if (newAccountChecker.Count() + 1 >= 5 && dogchecker.Count >= 5 && location.Count == 0)
                        {
                            await Navigation.PushAsync(new GetUsersLocation());
                        }
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                }
                else
                {
                    await Navigation.PushAsync(new TermsAndConditionPage());
                }
                
                UserDialogs.Instance.HideLoading();
            }
            catch (SQLite.SQLiteException)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}