using doghavenCapstone.MainPages;
using doghavenCapstone.MessagesComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace doghavenCapstone.LocalDBModel
{
    public class ConversationNames
    {
        public string conversationImage { get; set; }
        public string Name { get; set; }
        public string channelURL { get; set; }
        public string textMessage { get; set; }

        public ConversationNames()
        {
            OpenChat = new Command(GoToThisPage);
        }

        private void GoToThisPage(object obj)
        {
            MessagesPage._channelUrl = channelURL;
            MessagesPage.lstContent[0].Navigation.PushAsync(new EnterConversationPage());
        }

        public ICommand OpenChat { get; }
    }
}
