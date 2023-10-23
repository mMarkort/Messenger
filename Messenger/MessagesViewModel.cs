using Messenger.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    internal class MessagesViewModel : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        ObservableCollection<ChatMessagesItem> Messages = new ObservableCollection<ChatMessagesItem>();
    }
}
