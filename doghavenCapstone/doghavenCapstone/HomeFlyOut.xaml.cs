using doghavenCapstone.ClassHelper;
using doghavenCapstone.LocalDBModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeFlyOut : FlyoutPage
    {
        public HomeFlyOut()
        {
            InitializeComponent();
            setBreedingDistance();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void setBreedingDistance()
        {
            List<SettingsData> checker = null;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<SettingsData>();
                checker = conn.Table<SettingsData>().ToList();
                conn.Close();
            };

            if (checker.Count == 0)
            {
                string _default = "80";
                SettingsData settings = new SettingsData()
                {
                    breedingEstablishments = _default,
                    breedingKilometers = _default
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SettingsData>();
                    conn.Insert(settings);
                    conn.Close();
                };
                VariableStorage.breedingKilometers = _default;
                VariableStorage.breedingEstablishments = _default;
            }
            else
            {
                foreach(var c in checker)
                {
                    VariableStorage.breedingKilometers = c.breedingKilometers;
                    VariableStorage.breedingEstablishments = c.breedingEstablishments;
                }
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as HomeFlyOutFlyoutMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }
    }
}