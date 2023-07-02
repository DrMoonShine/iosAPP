using Friends.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Friends
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private static readonly HttpClient httpClient = new HttpClient();
        
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void log_Click(object sender, EventArgs e)
        {
            string userRegisterController = "http://u1693541.plsk.regruhosting.ru/api/auth/";
            userRegisterController = userRegisterController + Login.Text + "/" + UserPassword.Text;
            string result = await httpClient.GetStringAsync(userRegisterController);

       

            var oneUser = JsonConvert.DeserializeObject<User>(result);

           
            

            Preferences.Set("name", oneUser.NickName);
            Preferences.Set("id", oneUser.Id.ToString());

            
            if (Preferences.Get("name","none") == "none")
            {
                await DisplayAlert("Ошибка", "Неизвестная ошибка", "Ок");
            }
            else
            {
                await DisplayAlert("Уведомление", Preferences.Get("name", "none") + " успешно авторизован !", "Ок");
            }
           
            
        }
        private async void goReg_Click(object sender, EventArgs e)
        {
            var regPage = new RegisterPage();
            await Navigation.PushModalAsync(regPage);
        }
    }
}