using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Messenger
{
    public partial class ClientServer : ViewModelBase
    {
        public string IP { get; set; } = "94.241.175.205";
        public int Port { get; set; } = 5050;
        public string Nick { get; set; } = "";

        public string Password { get; set; } = "";

        public string? publicSeverKey { get; set; } //Публичный ключ сервера для шифрования
        public string? publicClientKey { get; set; }//Публичный ключ пользователя
        public string? privateClientKey { get; set; }//Приватный ключ клиента
        public string Chat
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string Message { get => GetValue<string>(); set => SetValue(value); }

        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public bool connected = false;

        public ClientServer()
        {

        }

        private void Listener()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (_client?.Connected == true)
                        {
                            var line = _reader?.ReadLine();
                            if (line != null)
                            {
                                Chat += line + "\n";
                            }
                            else
                            {
                                _client.Close();
                                Chat += "Connected error.\n";
                            }
                        }
                        Task.Delay(10).Wait();
                    }
                    catch (Exception ex)
                    {
                        Chat += ex.Message + "\n";
                    }
                }
            });
        }
        public AsyncCommand ConnectCommand
        { 
            get
            {
                return new AsyncCommand(() =>
                {
                   
                    return Task.Run(() =>
                    {
                        try
                        {
                            
                            _client = new TcpClient();
                            _client.Connect(IP, Port);
                            _reader = new StreamReader(_client.GetStream());
                            _writer = new StreamWriter(_client.GetStream());
                            Listener();
                            _writer.AutoFlush = true;
                            _writer.WriteLine("auth");
                            _writer.WriteLine($"Login: {Nick}");
                            _writer.WriteLine($"Password: {Password}");
                            var result = _reader.ReadLine();
                            if (result=="OK") connected= true;
                            else { MessageBox.Show(result); }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                }, () => _client is null || _client?.Connected == false);
            }
        }

        public AsyncCommand SendCommand
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        _writer?.WriteLine($"{Nick}: {Message}");
                        Message = "";
                    });
                }, () => _client?.Connected == true, !string.IsNullOrWhiteSpace(Message));
            }
        }
    }
}
