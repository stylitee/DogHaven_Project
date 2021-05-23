using System;
using doghavenCapstone.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteDogPage : ContentPage
    {
        public DeleteDogPage()
        {
            InitializeComponent();
        }

        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            var doginfo = await App.client.GetTable<dogInfo>().Where(x => x.id == App.dog_id).ToListAsync();
            dogInfo info = new dogInfo()
            {
                id = App.dog_id,
                dogName = doginfo[0].dogName,
                dogImage = doginfo[0].dogImage,
                dogGender = doginfo[0].dogGender,
                dogBreed_id = doginfo[0].dogBreed_id,
                dogPurpose_id = doginfo[0].dogPurpose_id,
                userid = App.user_id,
                createdAt = doginfo[0].createdAt,
                updatedAt = doginfo[0].updatedAt
            };

            await App.client.GetTable<dogInfo>().DeleteAsync(info);
            await Navigation.PopAsync();
            await DisplayAlert("Confirmation", "Dog information deleted succesfully", "Okay");
        }
    }
}