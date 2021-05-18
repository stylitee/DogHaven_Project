using doghavenCapstone.Model;
using doghavenCapstone.TabbedPageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstablishmentDetails : ContentPage
    {
        public static List<ContentPage> cnt = new List<ContentPage>();
        public static string latitude = "", longtitude = "";
        public EstablishmentDetails()
        {
            InitializeComponent();
            LoadDetails();
            loadPickers();
            ratings.IsVisible = false;
            btnSave.Text = "Rate Shop";
        }

        private void loadPickers()
        {
            pckRate.Items.Clear();
            pckRate.Items.Add("1");
            pckRate.Items.Add("2");
            pckRate.Items.Add("3");
            pckRate.Items.Add("4");
            pckRate.Items.Add("5");
        }

        private async void LoadDetails()
        {
            var details = await App.client.GetTable<dogRelatedEstablishments>().Where(x => x.id == RelatedShopsPage.store_id).ToListAsync();
            foreach(var c in details)
            {
                latitude = c.latitude;
                longtitude = c.longtitude;
                if (c.shopImage == "")
                {
                    imgShop.Source = "https://doghaven2storage.blob.core.windows.net/noimage/noimage.jpg";
                }
                else
                {
                    imgShop.Source = c.shopImage;
                }

                lblNameShop.Text = "Shop Name: " + c.nameOfShop;
                if(c.addtionalDesc == "")
                {
                    lblDesc.Text = "No description available";
                }
                else
                {
                    lblDesc.Text = c.addtionalDesc; 
                }

                if(c.facebookLink == "")
                {
                    lblFB.Text = "No facebook link saved";
                }
                else
                {
                    lblFB.Text = c.facebookLink;
                }
                
            }
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if(btnSave.Text == "Rate Shop")
            {
                ratings.IsVisible = true;
                btnSave.Text = "Save Rating";
            }
            else
            {
                if(pckRate.SelectedIndex == -1)
                {
                    DisplayAlert("Ops","No rating is selected","Okay");
                }
                else
                {
                    SaveRating();
                }
                ratings.IsVisible = false;
                btnSave.Text = "Rate Shop";

            }
        }

        private void SaveRating()
        {
            EstablishmentsRating establishment = new EstablishmentsRating()
            {
                id = Guid.NewGuid().ToString("N").Substring(0, 50),
                userid = App.user_id,
                establishment_id = RelatedShopsPage.store_id,
                rating = pckRate.Items[pckRate.SelectedIndex]
            };
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShowDirection());
        }
    }
}