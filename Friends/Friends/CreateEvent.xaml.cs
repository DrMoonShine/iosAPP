using Friends.Models;
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
    public partial class CreateEvent : ContentPage
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string userEvetnCreate = "http://u1693541.plsk.regruhosting.ru/api/createevent/create/";
        public CreateEvent()
        {
            InitializeComponent();
        }
        private async void Create_Click(object sender, EventArgs e)
        {
            //await DisplayAlert("aaaa", Preferences.Get("name", "default"), "Ok");
            string t = dateEvent.Date.ToString() + ":" + timeEvent.Time.ToString();
            string[] nums = t.Split(':','.',' ');
            DateTime dT = new DateTime(int.Parse(nums[2]),int.Parse(nums[1]), int.Parse(nums[0]), int.Parse(nums[6]), int.Parse(nums[7]), int.Parse(nums[8]));
            UserEvent newEvent = new UserEvent()
            {
                Name = eventName.Text,
                Description = discEvent.Text,
                DateStart = dT,
                Creator = Preferences.Get("name", "none"),
                Status = "Будет",
                SumUsers = int.Parse(sumUsersEvent.Text),
                Age = int.Parse(ageUsersEvent.Text),
                Tags = null

            };
            httpClient.DefaultRequestHeaders.Add("Accept","application/json");
            var response = await httpClient.PostAsync(userEvetnCreate,
                new StringContent(
                    JsonSerializer.Serialize(newEvent),
                    Encoding.UTF8, "application/json"));
            string res = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Результат", res, "ok");


        }
    }
}