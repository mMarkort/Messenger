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
            

            Regex eng = new Regex(@"^[A-Za-z\d_-]+$");
            var logins = App.usersTable.Select().AsEnumerable().Where(x => x["Login"].ToString() == loginText.Text).ToList();

            if (loginText.Text != ""&& !loginText.Text.Contains(" "))
            {
                if (logins.Count() == 0)
                {
                    if (passwordText.Password.Length < 6 | !(eng.Match(passwordText.Password).Success) | !(Regex.Match(passwordText.Password, @"\d").Success))
                    {
                        MessageBox.Show("Пароль должен содержать 6 или более символов\nДолжен иметь хотя бы одну цифру\nДолжен содержать английские символы");
                    }
                    else
                    {
                        if (passwordText.Password == passwordConfText.Password)
                        {
                            App.mainPage.Login.Background = new SolidColorBrush(Color.FromRgb(22, 49, 72));
                            App.mainPage.Signin.Background = new SolidColorBrush(Color.FromRgb(31, 71, 104));
                            App.mainPage.frameLogin.Navigate(App.loginPage);
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают");
                        }
                        
                    }
                    
                }
                else
                {
                    MessageBox.Show("Такой логин уже существует");
                }
            }

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
