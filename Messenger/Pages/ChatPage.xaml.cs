﻿using Messenger.Models;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;

        public ChatPage(string UserID)
        {
            userID = UserID;
            App.userID = UserID;
            InitializeComponent();



        }



        private void settings_Click(object sender, RoutedEventArgs e)
        {
            App.settingsPage = new SettingsPage();

            if (!App.clicked)
            {
                settingsFrame.Visibility = Visibility.Visible;
                bord.Visibility = Visibility.Collapsed;
                settingsFrame.Navigate(new SettingsPage());
                App.clicked = true;
            }
            else if (App.clicked)
            {
                settingsFrame.Visibility = Visibility.Collapsed;
                bord.Visibility = Visibility.Visible;
                App.clicked = false;
            }

        }



        private void files_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog(); 
        }

        private void _chatEditing(object sender, RoutedEventArgs e)
        {
            App.chatEditing = new ChatEditing();
            mainWindow.frameMenu2.Visibility = Visibility.Visible;
            mainWindow.frameMenu2.Navigate(App.chatEditing);
            App.chatEditing.Focus();
        }

        private void _searchDown(object sender, MouseButtonEventArgs e)
        {
            App.server.viewModel.searchRes.Clear();
            Task.Run(() => App.server.GetUsersAndChatsIDsWithUser.Execute(this)).Wait();
            bord.Visibility = Visibility.Collapsed;
            bord2.Visibility = Visibility.Visible;
            closeSearch.Visibility = Visibility.Visible;
        }

        private void closeSearchClick(object sender, RoutedEventArgs e)
        {
            closeSearch.Visibility = Visibility.Collapsed;
            bord.Visibility = Visibility.Visible;
            bord2.Visibility = Visibility.Collapsed;
            search.Text = "";
        }

        public void CreateChat(int ChatID, string ChatName, int UserID, dynamic a, ListViewItem item1, ListViewItem item2)
        {
            
            if (App.server.viewModel.Users.Where(x=>x.ChatID == ChatID.ToString()).Count() > 0)
            {
                App.server.ChatID = Convert.ToInt32(ChatID);

                Task.Run(() => App.server.GetBackground.Execute(this)).Wait();

                Task.Run(() => App.server.GetMessages.Execute(this)).Wait();

                item1.Background = new SolidColorBrush(Color.FromRgb(34, 76, 112));
                if (item2 != null)
                {
                    item2.Background = new SolidColorBrush(Color.FromRgb(22, 49, 72));
                }

                var c = App.server.viewModel.Users.Where(x => x.ChatID == ChatID.ToString()).Select(a => a.ChatName);

                MessangerName.Content = c.First();
                messagesView.Visibility = Visibility.Visible;
                chatBox.Visibility = Visibility.Visible;
                stub.Visibility = Visibility.Collapsed;
                EditChatButton.Visibility = Visibility.Visible;


                closeSearch.Visibility = Visibility.Collapsed;
                bord.Visibility = Visibility.Visible;
                bord2.Visibility = Visibility.Collapsed;

            }
            else
            {
                App.createChat = new CreateChat(ChatID, ChatName, null, 0, UserID);
                mainWindow.frameMenu2.Visibility = Visibility.Visible;
                mainWindow.frameMenu2.Navigate(App.createChat);
                App.createChat.Focus();
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.server.viewModel.SearchUsers(search.Text);
        }
    }
}
