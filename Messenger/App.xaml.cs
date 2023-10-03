using Messenger;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        public static string connectionString;
        public static SqlDataAdapter adapter;

        public static DataTable usersTable;


        public static ChatPage chatPage;
        public static ClientServer server;
        public static AuthPage authPage = new AuthPage();
        public static LoginPage loginPage = new LoginPage();
        public static MainPage mainPage = new MainPage();

        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection connectionSer = new HubConnectionBuilder().WithUrl("http://109.232.111.178:7018/chat").Build();

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string sql = "SELECT * FROM Users";

            usersTable = new DataTable();
            SqlConnection connection = null;

            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);

            adapter = new SqlDataAdapter(command);


            connection.Open();
            adapter.Fill(usersTable);


        }



    }
}

