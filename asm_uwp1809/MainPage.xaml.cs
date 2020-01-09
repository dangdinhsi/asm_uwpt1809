using asm_uwp1809.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace asm_uwp1809
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static Member currentLogin;
        private string currentTag = " ";
        public MainPage()
        {
            currentLogin = new Member();
            this.InitializeComponent();
            this.My_Frame.Navigate(typeof(View.MusicList));

        }

        private void btn_bar_Click(object sender, RoutedEventArgs e)
        {
            this.My_SplitView.IsPaneOpen = !this.My_SplitView.IsPaneOpen;

        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (currentTag == radio.Tag.ToString())
            {
                return;
            }
            switch (radio.Tag.ToString())
            {
                case "Home":
                    currentTag = "Home";
                    this.My_Frame.Navigate(typeof(View.Home));
                    break;
                case "My_account":
                    currentTag = "My_account";
                    this.My_Frame.Navigate(typeof(View.MemberDetail));
                    break;
                case "HotSong":
                    currentTag = "HotSong";
                    this.My_Frame.Navigate(typeof(View.MusicList));
                    break;
                case "Logout":
                    currentTag = "Logout";
                    Logout();
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(View.RegisterMember));
                    break;
            }

        }
        public async void Logout()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("token.txt") != null)
            {
                StorageFile file = await folder.GetFileAsync("token.txt");
                await file.DeleteAsync();
            }
        }
    }
}
