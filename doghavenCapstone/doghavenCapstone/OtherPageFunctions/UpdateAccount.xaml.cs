using Acr.UserDialogs;
using doghavenCapstone.ClassHelper;
using doghavenCapstone.Model;
using doghavenCapstone.PreventerPage;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.OtherPageFunctions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateAccount : ContentPage
	{
		List<userRole> roles = new List<userRole>();
		string _address_id = "", password = "";
		string url = "";
		string defaultImage = "";
		Stream dog_image = null;
		public UpdateAccount ()
		{
			InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
			LoadPickers();
		}

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
			AppHelpers.checkConnection(this, e);
		}

        private async void LoadPickers()
        {
			UserDialogs.Instance.ShowLoading("Please wait while loading");
			roles.Clear();
			pckrUserRole.Items.Clear();
			int flag = 1;
			var accountinfo = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
			var userType = await App.client.GetTable<userRole>().Where(x => x.id != "").ToListAsync();
			foreach(var c in userType)
            {
				var sellerchecker = await App.client.GetTable<SellerAdminRequest>().Where(x => x.user_id == App.user_id).ToListAsync();
				if(c.roleDescription != "Institution")
                {
					if(c.id != "2dskandlkdklsa")
                    {
						if (accountinfo[0].user_role_id != c.id)
						{
							roles.Add(c);
							pckrUserRole.Items.Add(c.roleDescription);
						}
					}
					if(c.id == "2dskandlkdklsa" && sellerchecker[0].admin_response == "ACCEPTED")
                    {
						roles.Add(c);
						pckrUserRole.Items.Add(c.roleDescription);
					}
				}
				if(userType.Count != flag)
                {
					flag++;
				}
				else
                {
					LoadInformations();
				}
            }
		}

        private async void LoadInformations()
        {
			if(roles.Count != 0)
            {
				var accountInfo = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
				var addressInfo = await App.client.GetTable<usersaddress>().Where(x => x.id == accountInfo[0].address_id).ToListAsync();
				var userType = await App.client.GetTable<userRole>().Where(x => x.id == accountInfo[0].user_role_id).ToListAsync();
				int index = roles.FindIndex(a => a.roleDescription == userType[0].roleDescription);
				
				defaultImage = accountInfo[0].userImage;
				txtUser_name.Text = accountInfo[0].username;
				txtFullname.Text = accountInfo[0].fullName;
				pckrUserRole.SelectedIndex = index;
				_address_id = addressInfo[0].id;
				txtStreetName.Text = addressInfo[0].streetname;
				txtBarangay.Text = addressInfo[0].barangay;
				txtCity.Text = addressInfo[0].city;
				txtProvince.Text = addressInfo[0].province;
				password = accountInfo[0].userPassword;
				imgUsersImage.Source = accountInfo[0].userImage;
				txtPhoneNumber.Text = accountInfo[0].phoneNumber;
			}
			UserDialogs.Instance.HideLoading();
		}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					await DisplayAlert("Ops!", "Your device is not supported to do this function", "Okay");
					return;
				}

				var mediaOptions = new PickMediaOptions()
				{
					PhotoSize = PhotoSize.Medium
				};
				var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

				if (selectedImageFile == null)
				{
					Acr.UserDialogs.UserDialogs.Instance.Toast("You haven't picked any image", new TimeSpan(2));
					return;
				}

				imgUsersImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

				dog_image = selectedImageFile.GetStream();
			}
            catch (Plugin.Media.Abstractions.MediaPermissionException)
            {

				await DisplayAlert("Ops","We need your permission to be able to access your photos","Okay");
            }
			
		}

        private void btnUpdate_Clicked(object sender, EventArgs e)
        {
			if (txtUser_name.Text == "" || txtFullname.Text == "" ||
			   txtStreetName.Text == "" || txtBarangay.Text == "" ||
			   txtCity.Text == "" || txtProvince.Text == "" || pckrUserRole.SelectedIndex == -1)
			{
				DisplayAlert("Ops", "Some fields are empty", "Okay");
			}
			else
			{
				UserDialogs.Instance.ShowLoading("Information is being processed, please wait");
				if (dog_image != null)
				{
					uploadUserImage(dog_image);
				}
				else
				{
					
					saveInformation();
				}
			}
        }

        private async void uploadUserImage(Stream dog_image)
        {
			try
			{
				var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=doghaven2storage;" +
													"AccountKey=Arp3X8RJ/LG4FMED/DqRMkWJn5Ba0IUhEdTJak6z5yOHcIsx+" +
													"97bgwfjuQNBLgmpt+0J0mjK8rcCGeeMJ/FT0Q==;EndpointSuffix=core.w" +
													"indows.net");
				var client = account.CreateCloudBlobClient();
				var container = client.GetContainerReference("useraccountimagescontainear");

				await container.CreateIfNotExistsAsync();

				var name = Guid.NewGuid().ToString();
				var blockBlob = container.GetBlockBlobReference($"{name}.jpg");

				await blockBlob.UploadFromStreamAsync(dog_image);
				url = blockBlob.Uri.OriginalString.ToString();
				infoInitializer();

			}
			catch (Exception)
			{
				UserDialogs.Instance.HideLoading();
				App.doginfo_flag = 0;
				await DisplayAlert("Warning", "Something went wrong with uploading the image", "Okay");
				return;
			}
		}

        private void infoInitializer()
        {
			if (url != "")
			{
				saveInformation();
			}
			UserDialogs.Instance.HideLoading();
		}

        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
			String val = txtPhoneNumber.Text; 

			if (val.Length > 11)
			{
				val = val.Remove(val.Length - 1);
				txtPhoneNumber.Text = val; 
			}
		}

        private async void saveInformation()
        {
			string _dogimage = "";
			if (url != "")
			{
				_dogimage = url;
			}
			else if (url == "")
			{
				_dogimage = defaultImage;
			}
			int index = roles.FindIndex(a => a.roleDescription == pckrUserRole.Items[pckrUserRole.SelectedIndex]);
			string role_id = roles[index].id;
			usersaddress address = new usersaddress()
			{
				id = _address_id,
				streetname = txtStreetName.Text,
				barangay = txtBarangay.Text,
				city = txtCity.Text,
				province = txtProvince.Text
			};

			accountusers user = new accountusers()
			{
				id = App.user_id,
				userImage = _dogimage,
				username = txtUser_name.Text,
				userPassword = password,
				fullName = txtFullname.Text,
				address_id = _address_id,
				user_role_id = role_id,
				phoneNumber = txtPhoneNumber.Text
			};

			await App.client.GetTable<usersaddress>().UpdateAsync(address);
			await App.client.GetTable<accountusers>().UpdateAsync(user);
			await DisplayAlert("Confirmation", "Account Succesfully Updated", "Okay");
			UserDialogs.Instance.HideLoading();
		}
    }
}