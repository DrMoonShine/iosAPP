using Friends.Models;
//using Newtonsoft.Json;
using System.Text.Json;
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
    public partial class UpdateDataPage : ContentPage
    {
        private readonly string userRegisterController = "http://u1693541.plsk.regruhosting.ru/api/auth/register/";
        public UpdateDataPage()
        {
            InitializeComponent();
        }
        public async void Update_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            UserDbo request = new UserDbo()
            {
                NickName = Preferences.Get("regName", "none"),
                Password = Preferences.Get("regPassword", "none"),
                Name = userName.Text,
                SecondName = userSecondName.Text,
                Age = int.Parse(userAge.Text),
                PhoneNumber = userPhone.Text,
                Email = userMail.Text,
                TgLink = userTgLink.Text,
                VkLink = userVkLink.Text,
                Discription = userDec.Text
            };
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await httpClient.PostAsync(userRegisterController,
                new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8, "application/json"));
            string result = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Результат", result, "Ok");
            Preferences.Remove("regName");
            Preferences.Remove("regPassword");
        }

    }
}