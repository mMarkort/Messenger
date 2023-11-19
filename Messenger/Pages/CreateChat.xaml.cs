using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для CreateChat.xaml
    /// </summary>
    public partial class CreateChat : Page
    {
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
        public int ChatID;
        public string ChatName;
        public string ChatAvatar;
        public int unrMessage;
        public int UserId;
        public CreateChat(int chatID, string chatName, string chatAvatar, int unrMessages, int UserID)
        {
            InitializeComponent();
            ChatID = chatID;
            ChatName = chatName;
            ChatAvatar = chatAvatar;
            unrMessage = unrMessages;
            UserId = UserID;
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source == this && e.LeftButton == MouseButtonState.Pressed)
            {
                mainWindow.frameMenu2.Visibility = Visibility.Collapsed;
                mainWindow.frameMenu2.Navigate(null);
            }
        }

        private void Create_Chat(object sender, RoutedEventArgs e)
        {
            App.server.AddChat(UserId, ChatName, ChatName);
            mainWindow.frameMenu2.Visibility = Visibility.Collapsed;
            mainWindow.frameMenu2.Navigate(null);

            App.chatPage.searchList.Visibility = Visibility.Collapsed;
            App.chatPage.bord2.Visibility = Visibility.Collapsed;

            App.chatPage.usersList.Visibility = Visibility.Visible;
            App.chatPage.bord.Visibility = Visibility.Visible;

            App.chatPage.closeSearch.Visibility = Visibility.Collapsed;

            App.chatPage.searchList.ItemsSource = App.server.viewModel.searchRes;

        }
    }
}
