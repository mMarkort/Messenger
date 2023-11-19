using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using Messenger.Models;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Messenger.Res
{

    public partial class CustomEvents : ResourceDictionary
    {
        public ListViewItem item2;
        public static byte[] imageBytes;
        private void ListViewItem_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            
            
            if (item != null && item.IsSelected && item != item2)
            {
                try
                {
                    dynamic a = App.chatPage.usersList.SelectedItem;

                    App.server.ChatID = Convert.ToInt32(a.ChatID);

                    Task.Run(() => App.server.GetBackground.Execute(this)).Wait();
                    
                    Task.Run(() => App.server.GetMessages.Execute(this)).Wait();
                    
                    item.Background = new SolidColorBrush(Color.FromRgb(34, 76, 112));
                    App.chatPage.MessangerName.Content = a.ChatName.ToString();
                    App.chatPage.messagesView.Visibility = Visibility.Visible;
                    App.chatPage.chatBox.Visibility = Visibility.Visible;
                    App.chatPage.stub.Visibility = Visibility.Collapsed;
                    App.chatPage.EditChatButton.Visibility = Visibility.Visible;
                    //background string

                    //if (!String.IsNullOrEmpty(App.server.BackgroundString))
                    //{
                    
                        
                    //}
                    //else
                    //{
                    //    App.chatPage.messagesView.Background = null;
                    //}

                }
                catch 
                {
                    try 
                    {
                        dynamic b = App.chatPage.searchList.SelectedItem;
                        App.chatPage.CreateChat(b.ChatID, b.ChatName, b.UserID);
                    }
                    catch 
                    {
                    
                    }
                }
            }
        }

        private void ListViewItem_RightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                //App.Users id = (App.Users)App.chatPage.usersList.SelectedItem;
                //App.users.Remove(id);
                MessageBox.Show("aaa");
            }
        }
    }
}
