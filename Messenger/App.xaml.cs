using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        public class Users
        {
            public Users(string Name) { UserName = Name; }
            public string UserName { get; set; }
        }
        

        public static ChatPage chatPage;
        public static AuthPage authPage;
        public static LoginPage loginPage;
        public static MainPage mainPage;
        public static List<Users> users = new List<Users>();

        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection connection = new HubConnectionBuilder().WithUrl("http://109.232.111.178:7018/chat").Build();
            users.Add(new Users("aboba"));
            users.Add(new Users("aboba1"));
            users.Add(new Users("aboba2"));
            users.Add(new Users("aboba3"));

        }



    }
}

