using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace Messenger.Res
{
    public partial class CustomEvents : ResourceDictionary
    {
        public ListViewItem item2;
        private void ListViewItem_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            
            if (item != null && item.IsSelected && item != item2)
            {
                try
                {
                    dynamic a = App.chatPage.usersList.SelectedItem;

                    App.server.ChatID = Convert.ToInt32(a.ChatID);

                    Task.Run(async () => App.server.GetMessages.Execute(this)).Wait();

                    item.Background = new SolidColorBrush(Color.FromRgb(34, 76, 112));
                    App.chatPage.MessangerName.Content = a.ChatName.ToString();
                    App.chatPage.messagesView.Visibility = Visibility.Visible;
                    App.chatPage.chatBox.Visibility = Visibility.Visible;
                    App.chatPage.stub.Visibility = Visibility.Collapsed;
                    item2 = item;
                }
                catch 
                {
                    
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
