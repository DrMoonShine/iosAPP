using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft;
//using Newtonsoft.Json;

using System.Net;
using System.Net.Http;
using Friends.Models;
using Xamarin.Essentials;

namespace Friends
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string userRegisterController = "http://u1693541.plsk.regruhosting.ru/api/auth/register/";
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void reg_clickReg_Click(object sender,EventArgs a)
        {
            Preferences.Set("regName", Login.Text);
            Preferences.Set("regPassword", userPassword.Text);
            var page = new UpdateDataPage();
            await Navigation.PushModalAsync(page);
           
        }
        private async void log_clickLog_Click(object sender, EventArgs e)
        {
            var loginPage = new LoginPage();
            await Navigation.PushModalAsync(loginPage);
        }
    }
}