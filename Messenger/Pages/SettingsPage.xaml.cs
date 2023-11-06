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
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
        
        public static byte[] imageBytes;

        public SettingsPage()
        {
            InitializeComponent();
            App.save = new SaveData();
            LanguageChoose.SelectedIndex = App.save.LoadData().Language;
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            App.mainPage = new MainPage();
            mainWindow.frameMenu.Navigate(new MainPage());
        }

        private void account_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                App.selectedFilePath = openFile.FileName;
                Change_Avatar();
            }
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


        public void Change_Avatar()
        {
            ImageBrush brush = new ImageBrush();

            imageBytes = Convert.FromBase64String(ComputeHash(App.selectedFilePath));


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
        }

        private void LanguageChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            App.save = new SaveData();

            
            if (LanguageChoose.SelectedIndex == 0)
            {
                App.save.Save(App.save.data = new SaveData.MyData() { Language = 0 });
                System.Windows.Application.Current.Resources.MergedDictionaries[1].Clear();
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Res/Languages.en-US.xaml") });
            }
            else if (LanguageChoose.SelectedIndex == 1)
            {
                
                App.save.Save(App.save.data = new SaveData.MyData() { Language = 1 });
                System.Windows.Application.Current.Resources.MergedDictionaries[1].Clear();
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Res/Languages.ru-RU.xaml") });
            }
        }
    }
}