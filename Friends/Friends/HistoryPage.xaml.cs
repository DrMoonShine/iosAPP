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
    public partial class HistoryPage : ContentPage
    {
        



        private async Task<List<UserEvent>> getAllResponse()
        {
            HttpClient all = new HttpClient();

            string url = "http://u1693541.plsk.regruhosting.ru/api/userevent/getres";
            var allResponse = await all.GetStringAsync(url);
            var allResponsetList = JsonConvert.DeserializeObject<List<Response>>(allResponse);

            url = "http://u1693541.plsk.regruhosting.ru/api/userevent";
            var allEvent = await all.GetStringAsync(url);
            var allEventList = JsonConvert.DeserializeObject<List<UserEvent>>(allEvent);

            var eventUser = new List<UserEvent>();

            for (int i = 0; i < allEventList.Count; i++)
            {
                for (int j = 0; j < allResponsetList.Count; j++)
                {
                    if (allResponsetList[j].UserEventId == allEventList[i].Id && allResponsetList[j].UserId == int.Parse(Preferences.Get("id","0")))
                    {
                        eventUser.Add(allEventList[i]);
                    }
                }
            }
            return eventUser;
        }
        private async Task<List<UserEvent>> getAllEvent()
        {
            HttpClient all = new HttpClient();


            string url = "http://u1693541.plsk.regruhosting.ru/api/userevent";
            var allEvent = await all.GetStringAsync(url);
            var allEventList = JsonConvert.DeserializeObject<List<UserEvent>>(allEvent);

            var eventUser = new List<UserEvent>();

            for (int i = 0; i < allEventList.Count; i++)
            {
                if(allEventList[i].Creator == Preferences.Get("name", "none"))
                {
                    eventUser.Add(allEventList[i]);
                }
            }
            return eventUser;
        }
        public HistoryPage()
        {
            InitializeComponent();
            
        }
        private async void CreateButtonEvent_Clicked(object sender, EventArgs e)
        {
            if (Preferences.Get("name", "none") == "none")
            {
                await DisplayAlert("Уведомление", "Для создания нового мероприятия авторизируйтесь !", "Ок");
            }
            else
            {
                var createPage = new CreateEvent();
                await Navigation.PushModalAsync(createPage);
            }
           
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            StackLayout stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Vertical;

            //TOP BAR с поиском
            AbsoluteLayout topBar = new AbsoluteLayout();


           /* StackLayout switchLayout = new StackLayout();
            switchLayout.Orientation = StackOrientation.Horizontal;
            Switch switchControl = new Switch { IsToggled = true };
            Label choose = new Label { Text = "Показать отклики " };
            switchLayout.Children.Add(switchControl);
            switchLayout.Children.Add(choose);*/

            //Градиент
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0, 0);
            linearGradientBrush.EndPoint = new Point(1, 1);
            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromHex("E77ABA"), 0));
            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromHex("#BC1778"), 1));

            topBar.Background = linearGradientBrush;
            topBar.HeightRequest = 60;



            List<UserEvent> events = new List<UserEvent>();
            events = await getAllResponse();
            CollectionView collectionViewEvent = new CollectionView();
  
            collectionViewEvent.Margin = new Thickness(10, 10, 10, 10);
            collectionViewEvent.ItemsSource = events;

            collectionViewEvent.ItemTemplate = new DataTemplate(() =>
            {
                Frame oneCard = new Frame()
                {
                    BorderColor = Color.DarkGray,
                    Padding = 0,            
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
                    
                    new RowDefinition{ Height = new GridLength(30) },
                    new RowDefinition{ Height = new GridLength(40) },
                    new RowDefinition{ Height = new GridLength(20) }
                    
                },
                    ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }

                };
                //Ячейка с создателем 
                Label creatorLabel = new Label();
                creatorLabel.Margin = new Thickness(0,2,5,2);
                creatorLabel.HorizontalOptions = LayoutOptions.End;
                creatorLabel.SetBinding(Label.TextProperty, "Creator");
                grid.Children.Add(creatorLabel, 1, 0);
                //Ячейка с Name
                Label lableName = new Label();
                lableName.Margin = 2;
                lableName.TextColor = Color.Black;
                lableName.SetBinding(Label.TextProperty, "Name");
                lableName.FontSize = 18;
                lableName.FontAttributes = FontAttributes.Bold;
                grid.Children.Add(lableName, 0, 0); // 1 - столбец 0 - строка
                //Ячейка с описанием
                Label labelDec = new Label();
                labelDec.SetBinding(Label.TextProperty, "Description");
                //labelDec.Text = itemEvent.Description;
                labelDec.Margin = 2;
                labelDec.TextColor = Color.Black;
                labelDec.FontSize = 12;
                grid.Children.Add(labelDec, 0, 1);
                Grid.SetColumnSpan(labelDec, 2);
                Grid.SetRowSpan(labelDec, 2);
                //Дата
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
                grid.Children.Add(labelDate, 1, 2);
                oneCard.Content = grid;
                return oneCard;
            });

            //Созданные
            /*List<UserEvent> eventsCreator = new List<UserEvent>();
            eventsCreator = await getAllEvent();
            CollectionView collectionCreator = new CollectionView();

            collectionCreator.Margin = new Thickness(10, 10, 10, 10);
            collectionCreator.ItemsSource = eventsCreator;

            collectionCreator.ItemTemplate = new DataTemplate(() =>
            {
                Frame oneCard = new Frame()
                {
                    BorderColor = Color.DarkGray,
                    Padding = 0,
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

                    new RowDefinition{ Height = new GridLength(30) },
                    new RowDefinition{ Height = new GridLength(40) },
                    new RowDefinition{ Height = new GridLength(20) }

                },
                    ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }

                };
                //Ячейка с создателем 
                Label creatorLabel = new Label();
                creatorLabel.Margin = new Thickness(0, 2, 5, 2);
                creatorLabel.HorizontalOptions = LayoutOptions.End;
                creatorLabel.SetBinding(Label.TextProperty, "Creator");
                grid.Children.Add(creatorLabel, 1, 0);
                //Ячейка с Name
                Label lableName = new Label();
                lableName.Margin = 2;
                lableName.TextColor = Color.Black;
                lableName.SetBinding(Label.TextProperty, "Name");
                lableName.FontSize = 18;
                lableName.FontAttributes = FontAttributes.Bold;
                grid.Children.Add(lableName, 0, 0); // 1 - столбец 0 - строка
                //Ячейка с описанием
                Label labelDec = new Label();
                labelDec.SetBinding(Label.TextProperty, "Description");
                //labelDec.Text = itemEvent.Description;
                labelDec.Margin = 2;
                labelDec.TextColor = Color.Black;
                labelDec.FontSize = 12;
                grid.Children.Add(labelDec, 0, 1);
                Grid.SetColumnSpan(labelDec, 2);
                Grid.SetRowSpan(labelDec, 2);
                //Дата
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
                grid.Children.Add(labelDate, 1, 2);
                oneCard.Content = grid;
                return oneCard;
            });
            */



            //Кнопка создать
            Button createButtonEvent = new Button();
            createButtonEvent.Margin = new Thickness(5);
            createButtonEvent.WidthRequest = 30;
            createButtonEvent.ImageSource = "icon_plus.png";
            createButtonEvent.BackgroundColor = Color.FromHex("#BC1778");
            createButtonEvent.BorderColor = Color.White;
            createButtonEvent.Margin = new Thickness(10);
            createButtonEvent.Clicked += CreateButtonEvent_Clicked;

            AbsoluteLayout.SetLayoutBounds(createButtonEvent, new Rectangle(1, 0, 60, 60));
            AbsoluteLayout.SetLayoutFlags(createButtonEvent, AbsoluteLayoutFlags.PositionProportional);
            //Название
            Label namePageLable = new Label();
            namePageLable.HeightRequest = 50;
            namePageLable.Text = "МОИ СОБЫТИЯ";
            namePageLable.FontSize = 40;
            namePageLable.TextColor = Color.White;
            namePageLable.FontFamily = "Uni Sans";
            namePageLable.FontAttributes = FontAttributes.Bold;
            namePageLable.Margin = new Thickness(10, 0, 0, 0);

     
            
            topBar.Children.Add(namePageLable);
            topBar.Children.Add(createButtonEvent);

            stackLayout.Children.Add(topBar);
            //switchLayout.Children.Add(switchLayout);
            stackLayout.Children.Add(collectionViewEvent);

            //stackLayout.Children.Add(collectionCreator);


            Button buttonTenLoad = new Button();
            buttonTenLoad.Text = "Загрузить еще";

            stackLayout.Children.Add(buttonTenLoad);
            Content = stackLayout;
        }
    }
}