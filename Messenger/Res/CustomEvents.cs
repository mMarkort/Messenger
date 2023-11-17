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

namespace Messenger.Res
{

    public partial class CustomEvents : ResourceDictionary
    {
        public ListViewItem item2;
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
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

                    Task.Run(async () => App.server.GetMessages.Execute(this)).Wait();

                    item.Background = new SolidColorBrush(Color.FromRgb(34, 76, 112));
                    App.chatPage.MessangerName.Content = a.ChatName.ToString();
                    App.chatPage.messagesView.Visibility = Visibility.Visible;
                    App.chatPage.chatBox.Visibility = Visibility.Visible;
                    App.chatPage.stub.Visibility = Visibility.Collapsed;
                    App.chatPage.EditChatButton.Visibility = Visibility.Visible;

                    //background string
                    if (!String.IsNullOrEmpty(App.server.AvatarString))
                    {
                        ImageBrush brush = new ImageBrush();
                        imageBytes = Convert.FromBase64String(App.server.AvatarString);
                        BitmapImage image2 = new BitmapImage();
                        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                        {
                            image2.BeginInit();
                            image2.CacheOption = BitmapCacheOption.OnLoad;
                            image2.StreamSource = memoryStream;
                            image2.EndInit();
                        }
                        brush.ImageSource = image2;
                        App.chatPage.messagesView.Background = brush;
                    }
                    else
                    {
                        App.chatPage.messagesView.Background = null;
                    }

                    foreach (var chat in App.server.viewModel.Users)
                    {
                        if (chat.ChatID == a.ChatID)
                        {
                            chat.unrMessages = 4;
                            App.server.viewModel.AddChatToList();

                        }
                    }


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
