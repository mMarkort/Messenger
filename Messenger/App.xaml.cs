using Messenger;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics.Contracts;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        public static string connectionString;
        public static SqlDataAdapter adapter;
        public static string selectedFilePath;
        public static int languageSelect;
        public static string userID;
        public static bool clicked = false;

        public static DataTable usersTable;

        public static ChatPage chatPage;
        public static ClientServer server;
        public static AccountPage account;
        public static ChatEditing chatEditing;
        public static CreateChat createChat;

        public static SaveData save;
        public static SettingsPage settingsPage;
        public static AuthPage authPage = new AuthPage();
        public static LoginPage loginPage = new LoginPage();
        public static MainPage mainPage = new MainPage();


        protected override void OnStartup(StartupEventArgs e)
        {
            //HubConnection connectionSer = new HubConnectionBuilder().WithUrl("http://109.232.111.178:7018/chat").Build();

            connectionString = "Data Source=94.241.175.205,1433;Initial Catalog=Messenger;Integrated Security=False;User Id=SA;Password=TheB3stPassw0rdF0rDB;";

            string sql = "SELECT * FROM Users";

            usersTable = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);

            adapter = new SqlDataAdapter(command);


            connection.Open();
            adapter.Fill(usersTable);
            SaveData data = new SaveData();

            if (data.LoadData().Language == 0) 
            {
                System.Windows.Application.Current.Resources.MergedDictionaries[1].Clear();
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Res/Languages.en-US.xaml") });
            }
            else if (data.LoadData().Language == 1)
            {
                System.Windows.Application.Current.Resources.MergedDictionaries[1].Clear();
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Res/Languages.ru-RU.xaml") });
            }

        }

        



    }
}

