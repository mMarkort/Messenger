using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.Models
{
    public class ViewModel : ObservableObject
    {
        public ObservableCollection<UserListModel> Users { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }

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
                this.AddChatToList();
            });


            //SqlConnection connection = null;
            //connection = new SqlConnection(App.connectionString);
            //SqlCommand getChats = new SqlCommand("SELECT Chat.ChatID, Users.UserID,Users.Login FROM Chat join userNchat on userNchat.ChatID = Chat.ChatID join Users on Users.UserID = userNchat.UserID", connection);
            //chatsAdapter = new SqlDataAdapter(getChats);
            //chatsTable = new DataTable();
            //connection.Open();
            //chatsAdapter.Fill(chatsTable);

            //var chatsToList = chatsTable.Select().AsEnumerable().Where(p => p["UserID"].ToString() == App.userID).Select(p => p["ChatID"]).ToList();
            //var opponents = chatsTable.Select().AsEnumerable().Where(p => chatsToList.Contains(p["ChatID"]) && p["UserID"].ToString() != App.userID).Select(p => new { ChatName = p["Login"] }).ToList();

            //foreach ( var opponent in opponents)
            //{
            //    Users.Add(new UserListModel
            //    {
            //        ChatName = opponent.ChatName.ToString()
            //    });
            //}


        }
        public void AddChatList(List<Dictionary<string, string>> chats)
        {
            foreach (var item in chats)
            {
                int a = String.IsNullOrEmpty(item["UnReMessage"]) ? 0 : Convert.ToInt32(item["UnReMessage"]);
                Users.Add(new UserListModel { ChatName = item["ChatName"], unrMessages=a ,ChatID= item["ChatID"] });
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

        public void AddChatToList()
        {
            Users.Add(new UserListModel
            {
                ChatID = "3100",
                ChatName = "Aboba",
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
                Messages.Add(new MessageModel { ChatID = chatID, dateTime = datettme, Message = Msg["msgCont"], MessageAutor = Msg["login"] });
                App.chatPage.messagesList.ScrollIntoView(Messages[Messages.Count() - 1]);
            }
        }

    }
}
