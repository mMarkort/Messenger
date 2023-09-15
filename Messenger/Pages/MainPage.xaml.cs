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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public LoginPage loginP = new LoginPage();
        public AuthPage authP = new AuthPage();
        public MainPage()
        {
            InitializeComponent();
            frameLogin.Navigate(loginP);
        }

        //в рамке меняется на логин окно
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            frameLogin.Navigate(loginP);
        }

        //в рамке меняется на окно регистрации
        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            frameLogin.Navigate(authP);
        }
    }
}
