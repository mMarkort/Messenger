﻿using System;
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
        public MainPage()
        {
            InitializeComponent();
            App.loginPage = new LoginPage();
            App.authPage = new AuthPage();
            frameLogin.Navigate(App.loginPage);
        }

        //в рамке меняется на логин окно
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login.Background = new SolidColorBrush(Color.FromRgb(22, 49, 72));
            Signin.Background = new SolidColorBrush(Color.FromRgb(31, 71, 104));
            frameLogin.Navigate(App.loginPage);
        }

        //в рамке меняется на окно регистрации
        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            Signin.Background = new SolidColorBrush(Color.FromRgb(22, 49, 72));
            Login.Background = new SolidColorBrush(Color.FromRgb(31, 71, 104));
            frameLogin.Navigate(App.authPage);
        }
    }
}
