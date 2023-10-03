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
        private void ListViewItem_LeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                dynamic a = App.chatPage.usersList.SelectedItem;
                App.chatPage.MessangerName.Content = a.ChatName.ToString();
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
