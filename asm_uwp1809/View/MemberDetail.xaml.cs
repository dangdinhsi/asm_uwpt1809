using asm_uwp1809.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace asm_uwp1809.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberDetail : Page
    {
        private Member currentMember;
        public MemberDetail()
        {
            this.currentMember = new Member();
            this.InitializeComponent();
            this.GetDetailsMember();
        }
        public async void GetDetailsMember()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync("token.txt");
            string content = await FileIO.ReadTextAsync(file);
            TokenResponse member_token = JsonConvert.DeserializeObject<TokenResponse>(content);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + member_token.Token);
            var response = client.GetAsync("https://2-dot-backup-server-003.appspot.com/_api/v2/members");
            var result = await response.Result.Content.ReadAsStringAsync();
            Member responseJsonMember = JsonConvert.DeserializeObject<Member>(result);
            this.name.Text = responseJsonMember.firstName + " " + responseJsonMember.lastName;
            this.txt_avatar.ProfilePicture = new BitmapImage(new Uri(responseJsonMember.avatar));
            this.txt_birthday.Text = responseJsonMember.birthday;
            this.txt_gender.CharacterSpacing = responseJsonMember.gender;
            this.txt_phone.Text = responseJsonMember.phone;
            this.txt_email.Text = responseJsonMember.email;
            int gender_member = responseJsonMember.gender;
            switch (gender_member)
            {
                case 0:
                    this.txt_gender.Text = "Female";
                    break;
                case 1:
                    this.txt_gender.Text = "Male";
                    break;
                case 2:
                    this.txt_gender.Text = "Other";
                    break;
            }
        }
    }
}
