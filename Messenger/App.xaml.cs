using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ChatPage chatPage;
        public static AuthPage authPage;
        public static LoginPage loginPage;
        public static MainPage mainPage;

        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection connection = new HubConnectionBuilder().WithUrl("http://109.232.111.178:7018/chat").Build();
            
        }
    }
}
