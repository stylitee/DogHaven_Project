using doghavenCapstone.LocalDBModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            loadPickers();
        }

        private void loadPickers()
        {
            List<SettingsData> checker = null;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<SettingsData>();
                checker = conn.Table<SettingsData>().ToList();
                conn.Close();
            };

            pckrDistanceBreed.Items.Add("80");
            pckrDistanceBreed.Items.Add("50");
            pckrDistanceBreed.Items.Add("30");
            pckrDistanceBreed.Items.Add("20");
            pckrDistanceBreed.Items.Add("10");

            pckrDistanceEstablishments.Items.Add("80");
            pckrDistanceEstablishments.Items.Add("50");
            pckrDistanceEstablishments.Items.Add("30");
            pckrDistanceEstablishments.Items.Add("20");
            pckrDistanceEstablishments.Items.Add("10");

            pckrDistanceBreed.SelectedItem = checker[0].breedingKilometers;
            pckrDistanceEstablishments.SelectedItem = checker[0].breedingEstablishments;
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<SettingsData> checker = null;
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SettingsData>();
                    checker = conn.Table<SettingsData>().ToList();
                    conn.Close();
                };

                SettingsData setting = new SettingsData()
                {
                    id = checker[0].id,
                    breedingKilometers = pckrDistanceBreed.Items[pckrDistanceBreed.SelectedIndex],
                    breedingEstablishments = pckrDistanceEstablishments.Items[pckrDistanceEstablishments.SelectedIndex]
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SettingsData>();
                    conn.Update(setting);
                    conn.Close();
                };

                DisplayAlert("Confirmation", "Changes Saves Succesfully", "Okay");
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}