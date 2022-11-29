using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using C971.Models;
using C971.Services;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppSettings : ContentPage
    {
        public AppSettings()
        {
            InitializeComponent();
        }

        private void ClearPreferences_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
        }

        void LoadSampleData_Clicked(object sender, EventArgs e)
        {
            DatabaseService.InsertSampleData();
            Navigation.PopToRootAsync();
        }

        async void ClearSampleData_Clicked(object sender, EventArgs e)
        {
            await DatabaseService.ClearSampleData();
            await Navigation.PopToRootAsync();
        }
    }
}