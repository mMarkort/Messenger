using Messenger.Models;
using Microsoft.VisualBasic;
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
        public string userID;
        public static bool clicked = false;

        public ChatPage(string UserID)
        {
            userID = UserID;
            App.userID = UserID;
            InitializeComponent();
            
        }



        private void settings_Click(object sender, RoutedEventArgs e)
        {
            App.settingsPage = new SettingsPage();

            if (!clicked)
            {
                settingsFrame.Visibility = Visibility.Visible;
                bord.Visibility = Visibility.Collapsed;
                settingsFrame.Navigate(new SettingsPage());
                clicked = true;
            }
            else if (clicked)
            {
                settingsFrame.Visibility = Visibility.Collapsed;
                bord.Visibility = Visibility.Visible;
                clicked = false;
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
            Messenger.Models.ViewModel viewModel = new Messenger.Models.ViewModel();
            viewModel.SendMessage(typingBox.Text);
        }



    }
}
