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
                Messages.Add(new MessageModel
                {
                    Message = MessageText,
                });
                MessageText = "";
                App.chatPage.messagesList.ScrollIntoView(Messages[Messages.Count() - 1]);
            });
            

            SqlConnection connection = null;
            connection = new SqlConnection(App.connectionString);
            SqlCommand getChats = new SqlCommand("SELECT Chat.ChatID, Users.UserID,Users.Login FROM Chat join userNchat on userNchat.ChatID = Chat.ChatID join Users on Users.UserID = userNchat.UserID", connection);
            chatsAdapter = new SqlDataAdapter(getChats);
            chatsTable = new DataTable();
            connection.Open();
            chatsAdapter.Fill(chatsTable);

            var chatsToList = chatsTable.Select().AsEnumerable().Where(p => p["UserID"].ToString() == App.userID).Select(p => p["ChatID"]).ToList();
            var opponents = chatsTable.Select().AsEnumerable().Where(p => chatsToList.Contains(p["ChatID"]) && p["UserID"].ToString() != App.userID).Select(p => new { ChatName = p["Login"] }).ToList();
            
            foreach ( var opponent in opponents)
            {
                Users.Add(new UserListModel
                {
                    UserName = opponent.ChatName.ToString()
                });
            }


        }

    }
}
