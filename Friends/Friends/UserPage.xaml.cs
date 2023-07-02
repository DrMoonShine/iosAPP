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
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(Preferences.Get("name", "none") == "none")
            {
                await DisplayAlert("Предупреждение", "Для отображение корректной информации авторизуйтесь!", "Ок");
            }
            else
            {
                HttpClient httpClient = new HttpClient();
                string userRegisterController = "http://u1693541.plsk.regruhosting.ru/api/auth/";
                userRegisterController = userRegisterController + Preferences.Get("name", "none");
                string result = await httpClient.GetStringAsync(userRegisterController);
                var oneUser = JsonConvert.DeserializeObject<User>(result);
                
                userNickName.Text = oneUser.NickName;
                userName.Text = "Имя: " + oneUser.Name;
                userSecondName.Text = "Фамилия: " + oneUser.SecondName;
                userAge.Text = "Возраст: " + oneUser.Age.ToString();
                userMail.Text = "E-mail: " + oneUser.Email;
                userTgLink.Text = "Telegram: " + oneUser.TgLink;
                userVkLink.Text = "Vk: " + oneUser.VkLink;
                userPhone.Text = "Телефон:" + oneUser.PhoneNumber;
                if(oneUser.Discription == null)
                {
                    userDec.Text = "Тут ничего нет";
                }
                else
                {
                    userDec.Text = oneUser.Discription;
                }
                
            }
        }
        private async void UserSettings_Click(object sender, EventArgs e)
        {
            if(Preferences.Get("name", "none") == "none")
            {
                var loginPage = new LoginPage();
                await Navigation.PushModalAsync(loginPage);
            }
            else
            {
                Preferences.Remove("name");
                Preferences.Remove("id");
                if (Preferences.Get("name", "none") == "none")
                {
                    await DisplayAlert("Уведомление", "Успешно", "Ок");
                }
            }   
        }
    }
}