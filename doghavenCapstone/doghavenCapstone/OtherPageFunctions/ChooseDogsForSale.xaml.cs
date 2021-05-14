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
    public partial class ChooseDogsForSale : ContentPage
    {
        List<dogInfo> lstdogs = new List<dogInfo>();
        public ChooseDogsForSale()
        {
            InitializeComponent();
            pckrLoad();
        }

        private async void pckrLoad()
        {
            lstdogs.Clear();
            pckrDogs.Items.Clear();
            pckrCompletePapers.Items.Add("Yes");
            pckrCompletePapers.Items.Add("No");
            pckrCompleteVaccines.Items.Add("Yes");
            pckrCompleteVaccines.Items.Add("No");

            var dogsForSale = await App.client.GetTable<dogInfo>().Where(x => x.userid == App.user_id && x.dogPurpose_id == "809udsan23d").ToListAsync();
            foreach(var c in dogsForSale)
            {
                lstdogs.Add(c);
                pckrDogs.Items.Add(c.dogName);
            }
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(txtAge.Text == "" || txtPrice.Text == "" ||
                   pckrDogs.SelectedIndex == -1 || pckrCompleteVaccines.SelectedIndex == -1 || pckrCompletePapers.SelectedIndex == -1)
                {
                    await DisplayAlert("Ops","Please complete all the fields provided","Okay");
                }
                else
                {
                    if(txtAge.Text == "8")
                    {
                        await DisplayAlert("Ops", "Ops! Dogs 8 weeks below are not allowed to be sell", "Okay");
                    }
                    else
                    {
                        var result = lstdogs.FindIndex(purpose => purpose.dogName ==
                                                              pckrDogs.Items[pckrDogs.SelectedIndex]);
                        var dog_id = lstdogs[result].id;
                        var sellerInfo = await App.client.GetTable<dogSeller>().Where(x => x.userid == App.user_id).ToListAsync();
                        DogPrice sell = new DogPrice()
                        {
                            id = Guid.NewGuid().ToString("N").Substring(0, 20),
                            doginfo_id = dog_id,
                            price = txtPrice.Text,
                            Age = txtAge.Text,
                            completeVaccines = pckrCompleteVaccines.Items[pckrCompleteVaccines.SelectedIndex],
                            withCompletePapers = pckrCompletePapers.Items[pckrCompletePapers.SelectedIndex],
                            seller_id = sellerInfo[0].id
                        };

                        await App.client.GetTable<DogPrice>().InsertAsync(sell);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {

        }
    }
}