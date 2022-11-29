using C971.Models;
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

    public partial class EditTerm : ContentPage
    {
        private readonly int _selectedTermId;

        public EditTerm()
        {
            InitializeComponent();
        }

        public EditTerm(Term term)
        {
            InitializeComponent();

            _selectedTermId = term.ID;
            TermID.Text = term.ID.ToString();
            TermTitle.Text = term.Title;
            StartDatePicker.Date = term.StartDate;
            EndDatePicker.Date = term.EndDate;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CourseCollectionView.ItemsSource = await DatabaseService.GetCourses(_selectedTermId);
        }

        async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var course = (Course)e.CurrentSelection.FirstOrDefault();
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new EditCourse(course));
            }
        }

        async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TermTitle.Text))
            {
                await DisplayAlert("Missing Title", "Please enter a title.", "OK");
                return;
            }
            if (DatabaseService.CompareDates(StartDatePicker.Date, EndDatePicker.Date) == false)
            {
                await DisplayAlert("Invalid Start Date", "Start Date Must be Before End Date!", "OK");
                return;
            }

            await DatabaseService.UpdateTerm(Int32.Parse(TermID.Text), TermTitle.Text, StartDatePicker.Date, EndDatePicker.Date);
            await Navigation.PopAsync();
        }

        async void CancelTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete Term and Related Courses?", "Delete this Term?", "Yes", "No");

            if (answer == true)
            {
                var id = int.Parse(TermID.Text);

                await DatabaseService.RemoveTerm(id);
                await DisplayAlert("Term Deleted", "Term Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Delete Canceled", "Term Was Not Deleted", "OK");
            }

            await Navigation.PopAsync();
        }

        async void AddCourse_Clicked(object sender, EventArgs e)
        {
            var termId = Int32.Parse(TermID.Text);

            await Navigation.PushAsync(new AddCourse(termId));
        }
    }
}