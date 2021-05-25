using Acr.UserDialogs;
using doghavenCapstone.LocalDBModel;
using doghavenCapstone.MainPages;
using doghavenCapstone.Model;
using SendBird;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace doghavenCapstone.MessagesComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterConversationPage : ContentPage
    {
        public ObservableCollection<ConversationNames> _conversationList = new ObservableCollection<ConversationNames>();
        string channel_id = "";
        SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler();
        public EnterConversationPage()
        {
            InitializeComponent();
            BindingContext = this;
            loadInformation();

        }

        private async void loadInformation()
        {
            string _urlImage = "", _txtImage = "";
            var conversationList = await App.client.GetTable<ConversationList>().Where(x => x.user_idOne == App.user_id || x.user_idTwo == App.user_id).ToListAsync();
            channel_id = conversationList[0].channelID;
            OpenChannel.GetChannel(channel_id, (OpenChannel openChannel, SendBirdException e) =>
            {
                if (e != null)
                {
                    UserDialogs.Instance.Toast("An error has occured", new TimeSpan(2));
                }

                openChannel.Enter((SendBirdException ex) =>
                {
                    if (ex != null)
                    {
                        UserDialogs.Instance.Toast("An error has occured", new TimeSpan(2));
                    }

                    UserDialogs.Instance.Toast("You entered the conversation", new TimeSpan(2));

                });

                PreviousMessageListQuery mListQuery = openChannel.CreatePreviousMessageListQuery();
                mListQuery.Load(30, true, (List<BaseMessage> messages, SendBirdException exe) =>
                {
                    if (exe != null)
                    {
                        UserDialogs.Instance.Toast("An error loading your messages", new TimeSpan(2));
                    }

                    foreach(var c in messages)
                    {
                        _conversationList.Add(new ConversationNames()
                        {
                            conversationImage = openChannel.CoverUrl,
                            textMessage = c.MessageId.ToString()
                        });
                    }
                });
            });

            

        }

        public ObservableCollection<ConversationNames> conversationList
        {
            get => _conversationList;
            set
            {
                _conversationList = value;
            }
        }



        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            var userinfo = await App.client.GetTable<accountusers>().Where(x => x.id == App.user_id).ToListAsync();
            OpenChannel.GetChannel(channel_id, (OpenChannel openChannel, SendBirdException ex) =>
            {
                if (ex != null)
                {
                    UserDialogs.Instance.Toast("An error has occured", new TimeSpan(2));
                }


                openChannel.SendUserMessage(txtMessage.Text, "Message Sent", (UserMessage userMessage, SendBirdException exe) =>
                {
                    if (exe != null)
                    {
                        // Handle error.
                    }
                    long messageId = userMessage.MessageId;
                    
                    _conversationList.Add(new ConversationNames()
                    {
                        conversationImage = userinfo[0].userImage,
                        textMessage = txtMessage.Text
                    });
                });
            });

        }
    }
}