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
    public partial class ChatEditing : Page
    {
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
        public static byte[] imageBytes;
        private bool _check = true;
        public ChatEditing()
        {
            InitializeComponent();
            //заменить на бэкграунд чата
            if (!String.IsNullOrEmpty(App.server.BackgroundString))
            {
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
                imageCircle.Source = image2;
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

            App.server.BackgroundString = ConvertedImage;

            Task.Run(async () => App.server.ChangeBackground.Execute(this)).Wait();
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
            imageCircle.Source = image2;
            App.chatPage.messagesView.Background = brush;
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
            Task.Run(async () => App.server.DeleteChat.Execute(this)).Wait();
            mainWindow.frameMenu2.Visibility = Visibility.Collapsed;
            mainWindow.frameMenu2.Navigate(null);
        }

    }
}

