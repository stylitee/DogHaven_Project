using Acr.UserDialogs;
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
                if(c.additionalDesc == "")
                {
                    lblDesc.Text = "No description available";
                }
                else
                {
                    lblDesc.Text = c.additionalDesc; 
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

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var checker = await App.client.GetTable<EstablishmentsRating>().Where(x => x.userid == App.user_id).ToListAsync();
            if(checker.Count == 0)
            {
                if (btnSave.Text == "Rate Shop")
                {
                    ratings.IsVisible = true;
                    btnSave.Text = "Save Rating";
                }
                else
                {
                    if (pckRate.SelectedIndex == -1)
                    {
                        await DisplayAlert("Ops", "No rating is selected", "Okay");
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading("Please wait while we save your rating");
                        SaveRating();
                    }
                    ratings.IsVisible = false;
                    btnSave.Text = "Rate Shop";

                }
            }
            else
            {
                if (btnSave.Text == "Rate Shop")
                {
                    ratings.IsVisible = true;
                    btnSave.Text = "Save Rating";
                }
                else
                {
                    if (pckRate.SelectedIndex == -1)
                    {
                        await DisplayAlert("Ops", "No rating is selected", "Okay");
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading("Please wait while we save your rating");
                        UpdateRating();
                    }
                    ratings.IsVisible = false;
                    btnSave.Text = "Rate Shop";

                }
            }

        }

        private async void UpdateRating()
        {
            string rate = pckRate.Items[pckRate.SelectedIndex].ToString();
            var checker = await App.client.GetTable<EstablishmentsRating>().Where(x => x.userid == App.user_id).ToListAsync();
            EstablishmentsRating establishment = new EstablishmentsRating()
            {
                id = checker[0].id,
                userid = App.user_id,
                establishment_id = checker[0].establishment_id,
                rating = rate
            };

            await App.client.GetTable<EstablishmentsRating>().UpdateAsync(establishment);

            computeRating();
        }

        private async void SaveRating()
        {
            string rate = pckRate.Items[pckRate.SelectedIndex].ToString();
            string _id = Guid.NewGuid().ToString("N").Substring(0, 30);

            EstablishmentsRating establishment = new EstablishmentsRating()
            {
                id = _id,
                userid = App.user_id,
                establishment_id = RelatedShopsPage.store_id,
                rating = rate
            };

            await App.client.GetTable<EstablishmentsRating>().InsertAsync(establishment);

            computeRating();

        }

        private async void computeRating()
        {
            //try ni ta dae pa na ttry
            List<int> results = new List<int>();
            var allRatings = await App.client.GetTable<EstablishmentsRating>().Where(x => x.establishment_id == RelatedShopsPage.store_id).ToListAsync();
            if(allRatings.Count >= 5)
            {
                foreach (var c in allRatings)
                {
                    results.Add(Convert.ToInt32(c.rating));
                }

                int total = 0;
                for (int i = 0; i < results.Count; i++)
                {
                    total += results[i];
                }

                double average = total / allRatings.Count;
                double roundoff = Math.Round(average, 2);

                var _estab = await App.client.GetTable<dogRelatedEstablishments>().Where(x => x.id == RelatedShopsPage.store_id).ToListAsync();
                dogRelatedEstablishments estab = new dogRelatedEstablishments()
                {
                    id = _estab[0].id,
                    shopImage = _estab[0].shopImage,
                    nameOfShop = _estab[0].nameOfShop,
                    latitude = _estab[0].latitude,
                    longtitude = _estab[0].longtitude,
                    rate = roundoff.ToString(),
                    additionalDesc = _estab[0].additionalDesc,
                    facebookLink = _estab[0].facebookLink
                };

                await App.client.GetTable<dogRelatedEstablishments>().UpdateAsync(estab);
                await DisplayAlert("Confirm", "Rating saved succesfully", "Okay");
            }
            UserDialogs.Instance.HideLoading();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShowDirection());
        }
    }
}