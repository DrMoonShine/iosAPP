using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Friends
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Preferences.Remove("countEvent");
            Preferences.Remove("creator");
            Preferences.Remove("regName");
            Preferences.Remove("regPassword");

            MainPage = new MainPage();
            //MainPage = new CreatorPage();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
