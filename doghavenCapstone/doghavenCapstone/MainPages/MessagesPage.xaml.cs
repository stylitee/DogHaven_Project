using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using doghavenCapstone.Model;
using Plugin.LocalNotification;
using SendBird;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesPage : ContentPage
    {
        public ObservableCollection<ConversationName> _conversationList = new ObservableCollection<ConversationList>();
        List<accountusers> userinfo = new List<accountusers>();
        SendBirdClient.ChannelHandler channel = new SendBirdClient.ChannelHandler();
        string token = "bbb00b761b8fc76589c4e5618c812ebd3f5bf466";
        int i = 0;
        public static string UNIQUE_HANDLER_ID = Guid.NewGuid().ToString("N").Substring(0, 10);
        public MessagesPage()
        {
            InitializeComponent();
            BindingContext = this;
            loadUserInfo();
            loadAccount();
            loadListOfConversation();
        }

        private async void loadListOfConversation()
        {
            var conversationList = await App.client.GetTable<ConversationList>().ToListAsync();
            foreach(var c in conversationList)
            {
                OpenChannel.GetChannel(c.channelID, (OpenChannel openChannel, SendBirdException e) =>
                {
                    if (e != null)
                    {
                        UserDialogs.Instance.Toast("An error has occured while retrieving your messages", new TimeSpan(2));
                    }

                    string ChannelName = openChannel.Name;
                });
            }
            /*_FoundDogList.Add(new FoundDogs()
            {
                id = c.id,
                dogImageSouce = dogImage_source,
                userid = c.userid,
                found_date = c.found_date,
                found_time = c.found_time,
                placeFound_latitude = c.placeFound_latitude,
                placeFound_longtitude = c.placeFound_longtitude,
                dogInfo_id = c.dogInfo_id,
                fullName = "Owner: " + full_Name,
                breedName = "Breed: " + breed_name,
                dateLost = "Date Lost: " + c.found_date,
                timeLost = "Time Lost: " + c.found_time,
                placeLost = fullFoundAddress
            });*/
        }

        public ObservableCollection<ConversationList> conversationList
        {
            get => _conversationList;
            set
            {
                _conversationList = value;
            }
        }
        private async void loadUserInfo()
        {
            userinfo.Clear();
            var accountInfo = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            foreach(var x in accountInfo)
            {
                userinfo.Add(x);
            }
        }

        private void loadAccount()
        {

            // The USER_ID below should be unique to your Sendbird application.
            SendBirdClient.Connect(userinfo[0].fullName, (User user, SendBirdException e) =>
            {
                if (e != null)
                {
                    UserDialogs.Instance.Toast("An error has occured connecting to your account", new TimeSpan(2));
                }
            });
            SendBirdClient.UpdateCurrentUserInfo(userinfo[0].fullName, userinfo[0].userImage, (SendBirdException e) =>
            {
                if (e != null)
                {
                    UserDialogs.Instance.Toast("An error has occured getting to your messages", new TimeSpan(2));
                }

            });
        }
    }
}