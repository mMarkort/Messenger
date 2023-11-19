using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using DevExpress.Mvvm.Native;

namespace Messenger.Models
{
    public class ViewModel : ObservableObject
    {
        public ObservableCollection<UserListModel> Users { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<SearchRes> searchRes { get; set; }
        public ObservableCollection<SearchRes> searchResFilter { get; set; }
        public ObservableCollection<UserListModel> EditedUsers { get; set; }

        public RelayCommand SendCommand { get; set; }
        public RelayCommand SendCommandToChats { get; set; }

        private string _message;

        public string MessageText
        {
            get { return _message; }
            set { 
                _message = value;
                OnPropertyChanged();
                }
        }




        public DataTable chatsTable;
        public static SqlDataAdapter chatsAdapter;

        public ViewModel()
        {
            Users = new ObservableCollection<UserListModel>();
            Messages = new ObservableCollection<MessageModel>();
            searchRes = new ObservableCollection<SearchRes>();
            searchResFilter = new ObservableCollection<SearchRes>();
            EditedUsers = new ObservableCollection<UserListModel>();

            SendCommand = new RelayCommand(o => 
            {
                App.server.Message = MessageText;
                App.server.SendMessageUs.Execute(this);
                this.AddMessage();
                //Messages.Add(new MessageModel
                //{
                //    Message = MessageText,
                //    MessageAutor = App.server.Nick
                //});
                //MessageText = "";
                //App.chatPage.messagesList.ScrollIntoView(Messages[Messages.Count() - 1]);
            });

            SendCommandToChats = new RelayCommand(a =>
            {
                //this.AddUserSearchList();
            });




        }
        public void AddChatList(List<Dictionary<string, string>> chats)
        {
            if (chats!=null)
            {
                foreach (var item in chats)
                {
                    int a = String.IsNullOrEmpty(item["UnReMessage"]) ? 0 : Convert.ToInt32(item["UnReMessage"]);
                    Users.Add(new UserListModel { ChatName = item["ChatName"], unrMessages = a, ChatID = item["ChatID"] });
                }
            }
            
        }
        public void AddMessageList(List<Dictionary<string, string>> msgs)
        {
            foreach (var item in msgs)
            {
                if (String.IsNullOrEmpty(item["chatID"]) || String.IsNullOrEmpty(item["datetime"]) || String.IsNullOrEmpty(item["msgCont"]) || String.IsNullOrEmpty(item["login"])) { continue; }
                int chatID = Convert.ToInt32(item["chatID"]);
                DateTime datettme;
                DateTime.TryParse(item["datetime"], out datettme);
                Messages.Add(new MessageModel { ChatID = chatID, dateTime = datettme, Message = item["msgCont"], MessageAutor = item["login"] });
            }
            if (Messages.Count()>0)
            {
                App.chatPage.messagesList.ScrollIntoView(Messages[Messages.Count() - 1]);

            }
        }

        public void AddChatToList(Dictionary<string, string> chat)
        {
            Users.Add(new UserListModel
            {
                ChatID = chat["chatId"],
                ChatName = chat["ChatName"],
                unrMessages = 0
            });

            EditedUsers.Add(new UserListModel
            {
                ChatID = chat["chatId"],
                ChatName = chat["ChatName"],
                unrMessages = 0
            });
        }
        public void AddMessage()
        {
            if (!String.IsNullOrEmpty(MessageText))
            {
                Messages.Add(new MessageModel
                {
                    Message = MessageText,
                    MessageAutor = App.server.Nick,
                    dateTime = DateTime.Now
                });
                MessageText = "";
                App.chatPage.messagesList.ScrollIntoView(Messages[Messages.Count() - 1]);
            }
        }
        public void AddMessageServer(Dictionary<string, string> Msg)
        {

            if (Msg.Count>0)
            {
                int chatID = Convert.ToInt32(Msg["chatID"]);
                DateTime datettme;
                DateTime.TryParse(Msg["datetime"], out datettme);
                if (App.server.ChatID==chatID)
                {
                    Messages.Add(new MessageModel { ChatID = chatID, dateTime = datettme, Message = Msg["msgCont"], MessageAutor = Msg["login"] });
                    App.chatPage.messagesList.ScrollIntoView(Messages[Messages.Count() - 1]);
                }
            }
        }
        //public void AddUnrMessage (string id)
        //{
        //    Users.FirstOrDefault(x =>x.ChatID==id).unrMessages= 0;
        //}
        public void AddUserSearchList(List<Dictionary<string, string>> usersus)
        {
            if (usersus.Count > 0)
            {
                foreach (var item in usersus)
                {
                    int chatID = Convert.ToInt32(item["ChatID"]);
                    string login = item["Login"];
                    int usID = Convert.ToInt32(item["userID"]);
                    searchRes.Add(new SearchRes
                    {
                        ChatName = login,
                        UserID = usID,
                        ChatID = chatID
                    });
                }
            }
        }
        public void RemoveChat(string ChatID)
        {
            var a = Users.First(x=>x.ChatID==ChatID);
            Users.Remove(a);
            EditedUsers = Users.Select(a=>a).ToObservableCollection();
            App.chatPage.usersList.ItemsSource = EditedUsers;
        }

        public void SearchUsers(string SearchText)
        {
            searchResFilter = searchRes.Where(x => x.ChatName.Contains(SearchText)).Select(a=>a).ToObservableCollection();
            App.chatPage.searchList.ItemsSource = searchResFilter;
        }

    }
}
