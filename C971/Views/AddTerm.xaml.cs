using C971.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTerm : ContentPage
    {
        public AddTerm()
        {
            InitializeComponent();
        }

        async void SaveTerm_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(TermTitle.Text))
            {
                await DisplayAlert("Missing Title", "Please enter a title.", "OK");
                return;
            }
            if (DatabaseService.CompareDates(StartDatePicker.Date, EndDatePicker.Date) == false) {
                await DisplayAlert("Invalid Start Date", "Start Date Must be Before End Date!", "OK");
                return;
            }

            await DatabaseService.AddTerm(TermTitle.Text, StartDatePicker.Date, EndDatePicker.Date);
            await Navigation.PopAsync();
        }

        async void CancelTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}