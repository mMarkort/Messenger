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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginBut_Click(object sender, RoutedEventArgs e)
        {
            //Когда будет подрублена БД чекать всё
            
            
            mainWindow.frameMenu.Navigate(new ChatPage());
 
        }

        //просто когда нажимаешь enter переносит на пароль
        private void loginText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                passwordText.Focus();
            }
        }

        //при enter логинит
        private void passwordText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginBut_Click(sender, e);
            }
        }

        private void loginText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
