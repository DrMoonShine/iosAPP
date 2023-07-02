using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

//using System.Text.Json;

using System.Net;
using System.Net.Http;

using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

namespace Friends
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventPage : ContentPage
    {

        private static readonly HttpClient allEvent = new HttpClient();
        private static int numberEvent = 1;
       


        private async Task<List<UserEvent>> getAllEvent()
        {
           
            int counter = int.Parse(Preferences.Get("countEvent", "0"));
            string url = "http://u1693541.plsk.regruhosting.ru/api/userevent";
            var result = await allEvent.GetStringAsync(url);

            var allEventList = JsonConvert.DeserializeObject<List<UserEvent>>(result);
            
            var fewEventList = new List<UserEvent>();
            if (counter < 10)
            {
                //первые 10 событий
                for (int i = 0; i < allEventList.Count; i++)
                {
                    fewEventList.Add(allEventList[i]);
                    if (fewEventList.Count >= 10)
                    {
                        /*counter = 10;
                       
                        Preferences.Remove("countEvent");
                        Preferences.Set("countEvent", counter.ToString());*/
                       
                        break;
                    }
                    //await DisplayAlert("Ok", counter.ToString(), "ok");
                }
            }
            else
            {
                for (int i = counter; i < allEventList.Count; i++)
                {
                    fewEventList.Add(allEventList[i]);
                    if (fewEventList.Count > 10)
                    {
                        counter += 10;
                        Preferences.Remove("countEvent");
                        Preferences.Set("countEvent", counter.ToString());
                        break;
                    }

                }
            }
           
            return fewEventList;    
        }

        
        public EventPage()
        {
            InitializeComponent();
           
        }
        Entry searchEntry;
        CollectionView collectionViewEvent;
        protected override async void OnAppearing()
        {
            base.OnAppearing();


            StackLayout stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Vertical;

            //TOP BAR с поиском
            AbsoluteLayout topBar = new AbsoluteLayout();




            //Градиент
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0);
            linearGradientBrush.EndPoint = new Point(1, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromHex("E77ABA"), 0));
            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromHex("#BC1778"), 1));

            topBar.Background = linearGradientBrush;









            //кнопка поиска
            Button searchButton = new Button()
            {
                Margin = new Thickness(5, 0, 5, 0),
                WidthRequest = 30,
                ImageSource = "icon_search.png",
                BackgroundColor = Color.FromHex("#BC1778"),
                BorderColor = Color.White,
                


            };
            searchButton.Clicked += SearchButton_Clicked;
            AbsoluteLayout.SetLayoutBounds(searchButton, new Rectangle(1, 0.5, 60, 60));
            AbsoluteLayout.SetLayoutFlags(searchButton, AbsoluteLayoutFlags.PositionProportional);
            //Строка поиска
            searchEntry = new Entry()
            {

                FontSize = 14,
                BackgroundColor = Color.White,
                WidthRequest = 300,
                Placeholder = "Поиск..."
            };
            Frame searchFrame = new Frame()
            {

                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderColor = Color.Gray,
                Padding = 0,
                Margin = new Thickness(20),
                CornerRadius = 15,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        searchEntry
                    }
                }
            };
            AbsoluteLayout.SetLayoutBounds(searchFrame, new Rectangle(0.5, 0, 300, 80));
            AbsoluteLayout.SetLayoutFlags(searchFrame, AbsoluteLayoutFlags.PositionProportional);

            topBar.Children.Add(searchFrame);
            topBar.Children.Add(searchButton);
            stackLayout.Children.Add(topBar);
            //Карточки
            List<UserEvent> events = new List<UserEvent>();
            events = await getAllEvent();
            collectionViewEvent = new CollectionView();
            collectionViewEvent.SelectionMode = SelectionMode.Single;
            collectionViewEvent.SelectionChanged += CollectionViewEvent_SelectionChanged;
            collectionViewEvent.Margin = new Thickness(10, 10, 10, 10);
            collectionViewEvent.ItemsSource = events;
      
            collectionViewEvent.ItemTemplate = new DataTemplate(() =>
            {
                Frame oneCard = new Frame()
                {
                    BorderColor = Color.DarkGray,
                    Padding = 0,
                    Margin = new Thickness(20),
                    HasShadow = true,
                    CornerRadius = 10

                };
                Grid grid = new Grid()
                {
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    Padding = 0,
                    RowDefinitions =
                {
                    new RowDefinition{ Height = new GridLength(35) },
                    new RowDefinition{ Height = new GridLength(45) },
                    new RowDefinition{ Height = new GridLength(45) },
                    new RowDefinition{ Height = new GridLength(12)  },
                    new RowDefinition{ Height = new GridLength(13) }
                },
                    ColumnDefinitions =
                {
                    new ColumnDefinition{Width = new GridLength(125) },
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }

                };
                //Ячейка с картинкой
                Frame imgFrame = new Frame()
                {
                    BorderColor = Color.DarkGray,
                    Padding = 0,
                    Margin = new Thickness(5),
                    CornerRadius = 10,
                    Content = new StackLayout
                    {

                    }
                };
                //BoxView greenBox = new BoxView { Color = Color.Green };
                grid.Children.Add(imgFrame);
                Grid.SetRowSpan(imgFrame, 3);
                //Ячейка с создателем 
                Label creatorLabel = new Label();
                creatorLabel.Margin = 2;
                creatorLabel.HorizontalOptions = LayoutOptions.Center;
                creatorLabel.SetBinding(Label.TextProperty, "Creator");
                grid.Children.Add(creatorLabel, 3,0);
                //Ячейка с Name
                Label lableName = new Label();
                lableName.Margin = 2;
                lableName.TextColor = Color.Black;
                lableName.SetBinding(Label.TextProperty, "Name");
                //lableName.Text = events.Name;
                lableName.FontSize = 18;
                lableName.FontAttributes = FontAttributes.Bold;
                grid.Children.Add(lableName, 1, 0); // 1 - столбец 0 - строка
                Grid.SetColumnSpan(lableName, 2);
                //Ячейка с описанием
                Label labelDec = new Label();
                labelDec.SetBinding(Label.TextProperty, "Description");
                //labelDec.Text = itemEvent.Description;
                labelDec.Margin = 2;
                labelDec.TextColor = Color.Black;
                labelDec.FontSize = 12;
                grid.Children.Add(labelDec, 1, 1);
                Grid.SetColumnSpan(labelDec, 3);
                Grid.SetRowSpan(labelDec, 2);
                //Дата
                Label labelDateText = new Label()
                {
                    Text = "Дата и Время",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    FontSize = 9,
                    TextColor = Color.Black,
                    Margin = 0

                };
                grid.Children.Add(labelDateText, 0, 4);
                Label labelDate = new Label()
                {
                    //Text = itemEvent.DateStart.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    TextColor = Color.Black,
                    Margin = 0
                    
                };
                labelDate.SetBinding(Label.TextProperty, "DateStart");
                grid.Children.Add(labelDate, 0, 5);
                //Статус
                Label labelTownText = new Label()
                {
                    Text = "Город",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    FontSize = 9,
                    TextColor = Color.Black,
                    Margin = 0

                };
                grid.Children.Add(labelTownText, 1, 4);
                Label labelStatus = new Label()
                {
                   // Text = itemEvent.Status,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    TextColor = Color.Black
                };
                labelStatus.SetBinding(Label.TextProperty, "Status");
                grid.Children.Add(labelStatus, 1, 5);
                //Возраст
                Label labelAgeText = new Label()
                {
                    Text = "Возраст",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    FontSize = 9,
                    TextColor = Color.Black,
                    Margin = 0

                };
                grid.Children.Add(labelAgeText, 2, 4);
                Label labelAge = new Label()
                {
                    //Text = itemEvent.Age + "+",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    TextColor = Color.Black
                };
                labelAge.SetBinding(Label.TextProperty, "Age");

                grid.Children.Add(labelAge, 2, 5);
                //Кол-во
                Label labelSumText= new Label()
                {
                    Text = "Кол-во участиков",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    FontSize = 9,
                    TextColor = Color.Black,
                    Margin = 0

                };
                grid.Children.Add(labelSumText, 3, 4);
                Label labelSum = new Label()
                {
                    //Text = itemEvent.SumUsers.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    TextColor = Color.Black
                };

                labelSum.SetBinding(Label.TextProperty, "SumUsers");
                grid.Children.Add(labelSum, 3, 5);
                /*BoxView selender = new BoxView();
                selender.BackgroundColor = Color.White;
                selender.HorizontalOptions = LayoutOptions.CenterAndExpand;
                selender.HeightRequest = 20;*/

                oneCard.Content = grid;
                return oneCard;
            });

            stackLayout.Children.Add(collectionViewEvent);
           

            Button buttonTenLoad = new Button();
            buttonTenLoad.Text = "Далее";
            buttonTenLoad.FontSize = 9;
            buttonTenLoad.HeightRequest = 20;
            buttonTenLoad.Clicked += Button_Clicked;
            Button buttonBack= new Button();
            buttonBack.Text = "Назад";
            buttonBack.FontSize = 9;
            buttonBack.HeightRequest = 40;
            buttonBack.Clicked += ButtonBack_Clicked;
            Grid btngrid = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = 0,
                RowDefinitions =
                {
                    new RowDefinition{ Height = new GridLength(60) },

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }

            };
            btngrid.Children.Add(buttonBack);
            btngrid.Children.Add(buttonTenLoad,1,0);
            stackLayout.Children.Add(btngrid);
            Content = stackLayout;

        }

        

        private async void CollectionViewEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Preferences.Remove("creator");
            var a = e.CurrentSelection[0] as UserEvent;
            bool check = await DisplayAlert("Подтверждение", "Вы точно хотите откликнутся ?", "Да","Нет");
            if (check)
            {
                if(Preferences.Get("id", "0") == "0")
                {
                    await DisplayAlert("Ошибка", "Для отклика нужно авторизироваться", "Ок");
                }
                else
                {
                    Response response = new Response()
                    {
                        UserId = int.Parse(Preferences.Get("id", "0")),
                        UserEventId = a.Id
                    };
                    HttpClient client = new HttpClient();
                    string url = "http://u1693541.plsk.regruhosting.ru/api/createevent/response/";
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    var res = await client.PostAsync(url,
                        new StringContent(
                            System.Text.Json.JsonSerializer.Serialize(response),
                            Encoding.UTF8, "application/json"));
                    Preferences.Set("creator", a.Creator);
                    string result = await res.Content.ReadAsStringAsync();
                    await DisplayAlert("Результат", result, "Ok");
                }
                
                
                var userPage = new CreatorPage();
                await Navigation.PushModalAsync(userPage);
            }
        }
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            HttpClient temp = new HttpClient();
            string eventCont = "http://u1693541.plsk.regruhosting.ru/api/userevent/search/";
            eventCont = eventCont + searchEntry.Text;
            string result = await temp.GetStringAsync(eventCont);



            var allResults = JsonConvert.DeserializeObject<List<UserEvent>>(result);
            collectionViewEvent.ItemsSource = allResults;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            int a = int.Parse(Preferences.Get("countEvent", "0"));
            a = a + 10;
            Preferences.Set("countEvent", a.ToString());
            var userEvents = await getAllEvent();
            
            if(userEvents.Count != 0)
            {
                collectionViewEvent.ItemsSource = userEvents;
            }
            

        }
        private async void ButtonBack_Clicked(object sender, EventArgs e)
        {
            int a = int.Parse(Preferences.Get("countEvent","0"));
            if(a != 0)
            {
                a = a - 10;
                Preferences.Set("countEvent", a.ToString());
                var userEvents = await getAllEvent();
                collectionViewEvent.ItemsSource = userEvents;
            }
            
            
        }
    }
}