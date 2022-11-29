using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C971.Models;
using C971.Services;
using Xamarin.Essentials;

namespace C971
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var dash = new Dashboard();
            var navPage = new NavigationPage(dash);
            MainPage = navPage;
        }

        protected override void OnStart()
        {
            if (Settings.FirstRun)
            {
                DatabaseService.InsertSampleData();
                Settings.FirstRun = false;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
