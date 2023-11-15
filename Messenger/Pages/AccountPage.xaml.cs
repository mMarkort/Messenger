using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
        public static byte[] imageBytes;
        private bool _check = true;
        public AccountPage()
        {
            InitializeComponent();
            nickname.Content = App.server.Nick;
            nickText.Text = App.server.Nick;
            if (!String.IsNullOrEmpty(App.server.AvatarString))
            {
                ImageBrush brush = new ImageBrush();
                imageBytes = Convert.FromBase64String(App.server.AvatarString);
                BitmapImage image2 = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    image2.BeginInit();
                    image2.CacheOption = BitmapCacheOption.OnLoad;
                    image2.StreamSource = memoryStream;
                    image2.EndInit();
                }

                brush.ImageSource = image2;
                imageCircle.Fill = brush;
            }
        }
        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source == this && e.LeftButton == MouseButtonState.Pressed)
            {
                mainWindow.frameMenu2.Visibility = Visibility.Collapsed;
                mainWindow.frameMenu2.Navigate(null);
            }
        }

        private void account_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif";
            try
            {
                if (openFile.ShowDialog() == true)
                {
                    App.selectedFilePath = openFile.FileName;
                    Change_Avatar();
                    ErrorText.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        public void Change_Avatar()
        {
            ImageBrush brush = new ImageBrush();

            var ConvertedImage = ComputeHash(App.selectedFilePath);

            imageBytes = Convert.FromBase64String(ConvertedImage);

            App.server.AvatarString = ConvertedImage;

            Task.Run(async () => App.server.ChangeAvatar.Execute(this)).Wait();
            //App.server.ChangeAvatar.ExecuteAsync(this).Wait();
            // Create a BitmapImage from the byte array
            BitmapImage image2 = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                image2.BeginInit();
                image2.CacheOption = BitmapCacheOption.OnLoad;
                image2.StreamSource = memoryStream;
                image2.EndInit();
            }

            brush.ImageSource = image2;
            imageCircle.Fill = brush;
            App.settingsPage = new SettingsPage();
            App.chatPage.settingsFrame.Navigate(App.settingsPage);
            App.settingsPage.imageCircle.Fill = brush;
        }


        public static string ComputeHash(string filePath)
        {
            BitmapImage image = new BitmapImage(new Uri(filePath));


            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(imageBytes);
        }

        private void _changeNick(object sender, RoutedEventArgs e)
        {
            
            if (_check)
            {
                nickText.IsReadOnly = false;
                nickText.Focus();
                nickText.Text = "";
                _check = false;
            }
            else if (!_check)
            {
                if (String.IsNullOrEmpty(nickText.Text) || nickText.Text.Contains(" ")) 
                {
                    nickText.Text = nickname.Content.ToString();
                    ErrorText2.Visibility = Visibility.Visible;
                    nickText.IsReadOnly = true;
                    _check = true;
                }
                else
                {
                    ErrorText2.Visibility = Visibility.Hidden;
                    nickname.Content = nickText.Text;
                    nickText.IsReadOnly = true;
                    _check = true;
                    App.settingsPage = new SettingsPage();
                    App.chatPage.settingsFrame.Navigate(App.settingsPage);
                    App.settingsPage.nickname.Content = nickname.Content;
                }
                
            }
        }

        private void imageCircle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                account_Click(sender, e);
            }
        }
    }
}

