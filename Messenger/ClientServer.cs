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
using System.Windows.Interop;

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
        public string ConfirmPassword { get; set; } = "";
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
                                        //if (ChatID.ToString() != JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(msg).Last()["chatID"])
                                        //{
                                        //    viewModel.AddUnrMessage(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(msg).Last()["chatID"]);   
                                        //}
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
                                    if (!String.IsNullOrEmpty(aStr))
                                    {
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
                                    else
                                    {
                                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            //byte[] imageBytes;
                                            //BackgroundString = aStr;
                                            //ImageBrush brush = new ImageBrush();
                                            //imageBytes = Convert.FromBase64String(App.server.BackgroundString);
                                            //BitmapImage image2 = new BitmapImage();
                                            //using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                                            //{
                                            //    image2.BeginInit();
                                            //    image2.CacheOption = BitmapCacheOption.OnLoad;
                                            //    image2.StreamSource = memoryStream;
                                            //    image2.EndInit();
                                            //}
                                            //brush.ImageSource = image2;
                                            App.chatPage.messagesView.Background = null;
                                        }));
                                    }
                                    
                                    
                                }else if (line == "GetUsersAndChatsIDsWithUser")
                                {
                                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        viewModel.AddUserSearchList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(_reader.ReadLine()));
                                    }
                                    ));
                                }else if (line == "NewChat")
                                {
                                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        viewModel.AddChatToList(JsonConvert.DeserializeObject<Dictionary<string, string>>(_reader?.ReadLine()));
                                    }
                                    ));
                                    
                                }else if (line== "DeleteChat")
                                {
                                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        string chatID = _reader?.ReadLine();
                                        MessageBox.Show("aa");
                                        viewModel.RemoveChat(chatID);
                                        
                                    }
                                    ));
                                    
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
                        else {
                            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                            { 
                                App.loginPage.ErrorLogin.Visibility = Visibility.Visible;
                            }));
                        }
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show(ex.Message);
                        //}
                    });
                }, () => _client is null || _client?.Connected == false);
            }
        }
        public AsyncCommand Registration
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        _client = new TcpClient();
                        _client.Connect(IP, Port);
                        _reader = new StreamReader(_client.GetStream());
                        _writer = new StreamWriter(_client.GetStream());

                        _writer.AutoFlush = true;
                        _writer?.WriteLine("Registration");
                        _writer?.WriteLine(JsonConvert.SerializeObject(new { Login = Nick, Password, ConfirmPassword }));
                        //_writer.WriteLine(Nick);
                        //_writer.WriteLine(Password);
                        var result = _reader?.ReadLine();

                        if (result == "OK")
                        {
                            var usId = _reader?.ReadLine();
                            _writer?.WriteLine("GetChats");
                            viewModel = new ViewModel();
                            string aStr = _reader.ReadLine();
                            viewModel.AddChatList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(aStr));
                            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                connected = false;
                                Exit();
                                App.loginPage = new LoginPage();
                                App.mainPage.Login.Background = new SolidColorBrush(Color.FromRgb(22, 49, 72));
                                App.mainPage.Signin.Background = new SolidColorBrush(Color.FromRgb(31, 71, 104));
                                App.mainPage.frameLogin.Navigate(App.loginPage);
                            }));
                            Listener();

                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                if (result == "Пароли не совпадают")
                                {
                                    App.authPage.Error1.Visibility = Visibility.Collapsed;
                                    App.authPage.Error2.Visibility = Visibility.Visible;
                                    App.authPage.Error3.Visibility = Visibility.Collapsed;
                                }
                                else if (result == "Логин не должен быть пустым и содержать пароли")
                                {
                                    App.authPage.Error1.Visibility = Visibility.Collapsed;
                                    App.authPage.Error2.Visibility = Visibility.Collapsed;
                                    App.authPage.Error3.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    App.authPage.Error1.Visibility = Visibility.Visible;
                                    App.authPage.Error2.Visibility = Visibility.Collapsed;
                                    App.authPage.Error3.Visibility = Visibility.Collapsed;
                                }
                            
                            }));
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
                        if (!String.IsNullOrEmpty(Message))
                        {
                            _writer.WriteLine("SendMessage");
                            var a = new { MessageText = Message, Time = DateTime.Now, SelectedChat = ChatID };

                            _writer.WriteLine(JsonConvert.SerializeObject(a));
                        }
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
        public AsyncCommand GetUsersAndChatsIDsWithUser
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        if (ChatID >= 0)
                        {
                            _writer?.WriteLine("GetUsersAndChatsIDsWithUser");
                        }

                    });
                }, () => _client?.Connected == true);
            }
        }
        public void AddChat(int ChelID, string ChelLogin, string ChatName)
        {
            string chlius = ChelID.ToString();
            Task.Run(() => 
            {
                _writer.WriteLine("AddChat");
                _writer.WriteLine(ChatName);
                _writer.WriteLine(chlius);
                _writer.WriteLine(ChelLogin);
            });
            //MessageBox.Show(JsonConvert.SerializeObject(new { ID = ChelID, Login = ChelLogin, Name = ChatName }));
        }
        public AsyncCommand DeleteChat
        {
            get
            {
                return new AsyncCommand(() =>
                {
                    return Task.Run(() =>
                    {
                        if (ChatID >= 0)
                        {
                            _writer?.WriteLine("DeleteChat");
                            _writer?.WriteLine(ChatID);
                        }

                    });
                }, () => _client?.Connected == true);
            }
        }
    }
}
