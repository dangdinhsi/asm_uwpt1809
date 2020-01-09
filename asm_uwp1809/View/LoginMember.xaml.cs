using asm_uwp1809.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace asm_uwp1809.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginMember : Page
    {
        private static Member currentLogin;
        public LoginMember()
        {
            this.InitializeComponent();
        }

        private async void Button_submit(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> login = new Dictionary<string, string>();
            login.Add("email", this.Email.Text);
            login.Add("password", this.Password.Password);
            HttpClient httpClient = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("https://2-dot-backup-server-003.appspot.com/_api/v2/members/authentication", stringContent).Result;
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await folder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(storageFile, responseContent);
                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));

            }
            else
            {
                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                if (errorResponse.error.Count > 0)
                {
                    foreach (var key in errorResponse.error.Keys)
                    {
                        var keys = this.FindName(key);
                        var value = errorResponse.error[key];
                        if (keys != null)
                        {
                            TextBlock textBlock = keys as TextBlock;
                            textBlock.Text =value;
                        }
                    }
                }
            }
        }

        public static async void Authentication()
        {
            //xac thuc da dang nhap = token
            currentLogin = new Member();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("token.txt") != null)
            {
                StorageFile file = await folder.GetFileAsync("token.txt");
                var tokenContent = await FileIO.ReadTextAsync(file);

                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(tokenContent);

                // get detail current member
                HttpClient client2 = new HttpClient();
                client2.DefaultRequestHeaders.Add("Authorization", "Basic " + token.Token);
                var resp = client2.GetAsync("https://2-dot-backup-server-003.appspot.com/_api/v2/members").Result;
                var memberInfoContent = await resp.Content.ReadAsStringAsync();

                Member memberInfoJson = JsonConvert.DeserializeObject<Member>(memberInfoContent);

                currentLogin.firstName = memberInfoJson.firstName;
                currentLogin.lastName = memberInfoJson.lastName;
                currentLogin.avatar = memberInfoJson.avatar;
                currentLogin.phone = memberInfoJson.phone;
                currentLogin.address = memberInfoJson.address;
                currentLogin.introduction = memberInfoJson.introduction;
                currentLogin.gender = memberInfoJson.gender;
                currentLogin.birthday = memberInfoJson.birthday;
                currentLogin.email = memberInfoJson.email;
                currentLogin.password = memberInfoJson.password;
                currentLogin.status = memberInfoJson.status;
                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
 
            }
        }
        private void Sign_up(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(View.RegisterMember));
        }
    }
}
