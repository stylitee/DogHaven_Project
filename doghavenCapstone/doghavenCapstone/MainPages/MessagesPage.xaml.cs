using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using doghavenCapstone.LocalDBModel;
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
        public static string _channelUrl = "", userimage = "";
        public ObservableCollection<ConversationNames> _conversationList = new ObservableCollection<ConversationNames>();
        public static List<ContentPage> lstContent = new List<ContentPage>();
        SendBirdClient.ChannelHandler channel = new SendBirdClient.ChannelHandler();
        //string token = "bbb00b761b8fc76589c4e5618c812ebd3f5bf466";
        int i = 0;
        public static string UNIQUE_HANDLER_ID = Guid.NewGuid().ToString("N").Substring(0, 10);
        public MessagesPage()
        {
            InitializeComponent();
            lstContent.Clear();
            lstContent.Add(this);
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            lstContent.Clear();
            lstContent.Add(this);
            loadAccount();
            loadListOfConversation();
            base.OnAppearing();
        }

        private async void loadListOfConversation()
        {
            var conversationList = await App.client.GetTable<ConversationList>().Where(x => x.user_idOne == App.user_id || x.user_idTwo == App.user_id).ToListAsync();
            var myinformation = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            foreach (var c in conversationList)
            {
                OpenChannel.GetChannel(c.channelID, (OpenChannel openChannel, SendBirdException e) =>
                {
                    if (e != null)
                    {
                        UserDialogs.Instance.Toast("An error has occured while retrieving your messages", new TimeSpan(2));
                    }

                    // Through the "openChannel" parameter of the callback function,
                    // the open channel object identified with the CHANNEL_URL is returned by Sendbird server,
                    // and you can get the open channel's data from the result object.
                    string ChannelName = openChannel.Name;
                    _conversationList.Clear();
                    userimage = myinformation[0].userImage;
                    _conversationList.Add(new ConversationNames()
                    {
                        conversationImage = openChannel.CoverUrl,
                        Name = openChannel.Name,
                        channelURL = openChannel.Url,
                    });

                });
            }
        }

        public ObservableCollection<ConversationNames> conversationList
        {
            get => _conversationList;
            set
            {
                _conversationList = value;
            }
        }

        private void loadAccount()
        {
            //var accountusers = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            // The USER_ID below should be unique to your Sendbird application.
            SendBirdClient.Connect(App.user_id, (User user, SendBirdException e) =>
            {
                if (e != null)
                {
                    // Handle error.
                }

                // The user is connected to Sendbird server.
            });
            /*SendBirdClient.Connect(accountusers[0].fullName, (User user, SendBirdException e) =>
            {
                if (e != null)
                {
                    UserDialogs.Instance.Toast("An error has occured connecting to your account", new TimeSpan(2));
                }
            });
            SendBirdClient.UpdateCurrentUserInfo(accountusers[0].fullName, accountusers[0].userImage, (SendBirdException e) =>
            {
                if (e != null)
                {
                    UserDialogs.Instance.Toast("An error has occured getting to your messages", new TimeSpan(2));
                }

            });*/
        }
    }
}