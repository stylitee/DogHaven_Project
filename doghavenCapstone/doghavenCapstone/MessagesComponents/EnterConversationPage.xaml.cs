using Acr.UserDialogs;
using doghavenCapstone.LocalDBModel;
using doghavenCapstone.MainPages;
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
        public EnterConversationPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadInformations();
        }

        private void LoadInformations()
        {
            OpenChannel.GetChannel(MessagesPage._channelUrl, (OpenChannel openChannel, SendBirdException e) =>
            {
                if (e != null)
                {
                    UserDialogs.Instance.Toast("An error has occured", new TimeSpan(2));
                }

                openChannel.Enter((SendBirdException ex) =>
                {
                    if (e != null)
                    {
                        UserDialogs.Instance.Toast("Unable to join the conversation an error has occured", new TimeSpan(2));
                    }

                    // The current user successfully enters the open channel,
                    // and can chat with other users in the channel by using APIs.
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

        private void btnSend_Clicked(object sender, EventArgs e)
        {
            OpenChannel.SendUserMessage(, DATA, (UserMessage userMessage, SendBirdException e) =>
            {
                if (e != null)
                {
                    // Handle error.
                }

                long messageId = userMessage.MessageId;
            });
        }
    }
}