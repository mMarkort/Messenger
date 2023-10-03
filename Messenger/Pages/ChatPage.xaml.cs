using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public static string userID;
        public static DataTable chatsTable;
        public static SqlDataAdapter chatsAdapter;

        public ChatPage(string UserID)
        {
            InitializeComponent();
            userID = UserID;
            SqlConnection connection = null;
            connection = new SqlConnection(App.connectionString);
            SqlCommand getChats = new SqlCommand("SELECT Chat.ChatID, Users.UserID,Users.Login FROM Chat join userNchat on userNchat.ChatID = Chat.ChatID join Users on Users.UserID = userNchat.UserID", connection);
            chatsAdapter = new SqlDataAdapter(getChats);
            chatsTable = new DataTable();
            connection.Open();
            chatsAdapter.Fill(chatsTable);

            var chatsToList = chatsTable.Select().AsEnumerable().Where(p => p["UserID"].ToString() == userID).Select(p => p["ChatID"]).ToList();
            var opponents = chatsTable.Select().AsEnumerable().Where(p => chatsToList.Contains(p["ChatID"]) && p["UserID"].ToString() != userID).Select(p => new { ChatName = p["Login"] }).ToList();

            usersList.ItemsSource = opponents; 
        }



        private void settings_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void typingBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (typingBox.Text == "" )
            {
                placeholder.Visibility = Visibility.Visible;
            }
            else
            {
                placeholder.Visibility =Visibility.Hidden;
            }

        }

        private void files_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog(); 
        }

        private void typingBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                send_Click(sender, e);
                typingBox.Text = "";
            }
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            
            App.server.Message = typingBox.Text;
            App.server.SendCommand.ExecuteAsync(this);
        }
    }
}
