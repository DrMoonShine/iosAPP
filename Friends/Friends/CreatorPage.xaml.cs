using Friends.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Friends
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage : ContentPage
    {
        public CreatorPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Preferences.Get("creator", "none") == "none")
            {

            }
            else
            {
                HttpClient httpClient = new HttpClient();
                string userRegisterController = "http://u1693541.plsk.regruhosting.ru/api/auth/";
                userRegisterController = userRegisterController + Preferences.Get("creator", "none");
                string result = await httpClient.GetStringAsync(userRegisterController);
                var oneUser = JsonConvert.DeserializeObject<User>(result);

                userNickName.Text = oneUser.NickName;
                userName.Text = "Имя: " + oneUser.Name;
                userSecondName.Text = "Фамилия: " + oneUser.SecondName;
                userAge.Text = "Возраст: " + oneUser.Age.ToString();
                userMail.Text = "E-mail: " + oneUser.Email;
                userTgLink.Text = "Telegram: " + oneUser.TgLink;
                userVkLink.Text = "Vk: " + oneUser.VkLink;
                if (oneUser.Discription == null)
                {
                    userDec.Text = "Тут ничего нет";
                }
                else
                {
                    userDec.Text = oneUser.Discription;
                }

            }
        }
        
    }
}