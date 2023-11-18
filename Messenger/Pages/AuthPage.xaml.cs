using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public AuthPage()
        {
            InitializeComponent();
            
        }

        private void authBut_Click(object sender, RoutedEventArgs e)
        {

            App.server = new ClientServer();
            App.server.Nick = loginText.Text;
            App.server.Password = passwordText.Password;
            App.server.ConfirmPassword = passwordConfText.Password;
            App.server.Registration.ExecuteAsync(this);

        }
        private void loginText_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                passwordText.Focus();
            }
        }

        private void passwordText_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                passwordConfText.Focus();
            }
        }
        private void passwordConfText_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                authBut_Click(sender, e);
            }
        }
    }
}
