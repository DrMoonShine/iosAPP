using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using System.Threading.Tasks;
using Friends.Models;

namespace Friends.Controllers
{
    internal class EventController
    {
        internal static async Task<List<string>> getAllEvent(int numberEvent, HttpClient allEvent)
        {
            allEvent = new HttpClient();
            List<UserEvent> events = new List<UserEvent>();
            List<string> stringsInTable = new List<string>();
            string tenUrl;
            string temp;

            HttpResponseMessage response;
            while (stringsInTable.Count < 10)
            {
                tenUrl = "http://u1657590.plsk.regruhosting.ru/api/userevent/";
                response = await allEvent.GetAsync(tenUrl + numberEvent);
                temp = await response.Content.ReadAsStringAsync();
                if (temp.Contains("Not Found"))
                {
                    break;
                }
                stringsInTable.Add(temp);
                numberEvent++;
            }

            //await DisplayAlert("Тест", stringsInTable.Count.ToString(), "Ок");
            return stringsInTable;
        }
    }
}
