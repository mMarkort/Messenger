using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Messenger.Models;
using System.Text.Json.Serialization;

using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Messenger
{
    public partial class ClientServer : ViewModelBase
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public string IP { get; set; } = "94.241.175.205";
        public int Port { get; set; } = 5050;
        public string Nick { get; set; } = "";
        public bool Status { get; set; } = true;
        public string AvatarString { get; set; } = "";
        public string BackgroundString { get; set; } = "";
        public string Password { get; set; } = "";
        public int ChatID { get; set; } = 0;
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

        public ViewModel viewModel;

        public bool IsBackChange;
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
                            if (!String.IsNullOrEmpty(line))
                            {
                                if (line == "Message")
                                {
                                    var msg = _reader?.ReadLine();
                                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        viewModel.AddMessageServer(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(msg).Last());
                                        //MainWindow mainWindowSus = Application.Current.MainWindow as MainWindow;
                                        //App.chatPage.DataContext = viewModel;
                                    }));
                                }
                                else if (line == "MessagesFromChat")
                                {
                                    var aStr = _reader?.ReadLine();
                                    if (!String.IsNullOrEmpty(aStr))
                                    {
                                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            viewModel.Messages.Clear();
                                            var a = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(aStr);
                                            if (a.Count>0)
                                            {
                                                viewModel.AddMessageList(a);

                                            }
                                            //MainWindow mainWindowSus = Application.Current.MainWindow as MainWindow;
                                            //App.chatPage.DataContext = viewModel;
                                        }));
                                    }
                                }else if(line== "BackGround")
                                {
                                    var aStr = _reader.ReadLine();
                                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        byte[] imageBytes;
                                        BackgroundString = aStr;
                                        ImageBrush brush = new ImageBrush();
                                        imageBytes = Convert.FromBase64String(App.server.BackgroundString);
                                        BitmapImage image2 = new BitmapImage();
                                        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                                        {
                                            image2.BeginInit();
                                            image2.CacheOption = BitmapCacheOption.OnLoad;
                                            image2.StreamSource = memoryStream;
                                            image2.EndInit();
                                        }
                                        brush.ImageSource = image2;
                                        App.chatPage.messagesView.Background = brush;
                                    }));
                                    
                                }
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
        public  AsyncCommand ConnectCommand
        { 
            get
            {
                return new AsyncCommand(() =>
                {
                   
                    return Task.Run(() =>
                    {
                        //try
                        //{
                            
                        _client = new TcpClient();
                        _client.Connect(IP, Port);
                        _reader = new StreamReader(_client.GetStream());
                        _writer = new StreamWriter(_client.GetStream());
                            
                        _writer.AutoFlush = true;
                        _writer.WriteLine("auth");
                        _writer.WriteLine($"Login: {Nick}");
                        _writer.WriteLine($"Password: {Password}");
                        //_writer.WriteLine(Nick);
                        //_writer.WriteLine(Password);
                        var result =_reader?.ReadLine();
                            
                        if (result == "OK")
                        {
                            var usId = _reader?.ReadLine();
                                
                            var IsAvatar = _reader?.ReadLine()=="true";
                            if(IsAvatar){
                                AvatarString=_reader?.ReadLine();
                            }
                            _writer.WriteLine("GetChats");
                            viewModel = new ViewModel();
                            string aStr = _reader.ReadLine();
                            viewModel.AddChatList(JsonConvert.DeserializeObject<List<Dictionary<string,string>>>(aStr));
                            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                MainWindow mainWindowSus = Application.Current.MainWindow as MainWindow;
                                connected = true;
                                App.chatPage = new ChatPage(usId);
                                App.chatPage.DataContext = viewModel;
                                mainWindowSus.frameMenu.Navigate(App.chatPage);
                            }));
                            Listener();

                        }
                        else { MessageBox.Show(result); }
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show(ex.Message);
                        //}
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
        public AsyncCommand ChangeAvatar
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        _writer?.WriteLine("ChangeAvatar");
                        _writer?.WriteLine(AvatarString);
                    });
                },() => _client?.Connected == true);
            }
        }
        public AsyncCommand SendMessageUs
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {

                        _writer.WriteLine("SendMessage");
                        var a = new { MessageText = Message, Time = DateTime.Now, SelectedChat = ChatID };
                    
                        _writer.WriteLine(JsonConvert.SerializeObject(a));

                        
                    });
                }, () => _client?.Connected == true);
            }
        }
        public AsyncCommand GetMessages
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        if (ChatID>=0)
                        {
                            _writer?.WriteLine("GetMessagesFromChat");
                            _writer?.WriteLine(ChatID);
                        }
                        
                    });
                }, () => _client?.Connected == true);
            }
        }
        public AsyncCommand GetBackground
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        if (ChatID >= 0)
                        {
                            _writer?.WriteLine("GetBackGround");
                            _writer?.WriteLine(ChatID);
                        }

                    });
                }, () => _client?.Connected == true);
            }
        }
        public AsyncCommand ChangeBackground
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        if (ChatID >= 0)
                        {
                            _writer?.WriteLine("ChangeBackground");
                            _writer?.WriteLine(BackgroundString);
                            _writer?.WriteLine(ChatID);
                        }

                    });
                }, () => _client?.Connected == true);
            }
        }
        public AsyncCommand ChangeNick
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        if (ChatID >= 0)
                        {
                            _writer?.WriteLine("ChangeNick");
                            _writer?.WriteLine(Nick);
                        }

                    });
                }, () => _client?.Connected == true);
            }
        }
        public void Exit()
        {
            _client.Client.Close();
        }
    }
}
