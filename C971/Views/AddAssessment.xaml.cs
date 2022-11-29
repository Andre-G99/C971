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
    public partial class AddAssessment : ContentPage
    {
        private readonly int _selectedCourseId;
        public AddAssessment()
        {
            InitializeComponent();
        }

        public AddAssessment(int courseId)
        {
            InitializeComponent();
            _selectedCourseId = courseId;
        }

        async void SaveAssessment_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(AssessmentTitle.Text))
            {
                await DisplayAlert("Missing Title", "Please enter a title.", "OK");
                return;
            }
            if (AssessmentTypePicker.SelectedItem == null)
            {
                await DisplayAlert("Missing Assessment Type", "Please pick a type.", "OK");
                return;
            }
            if (DatabaseService.GetObjectiveAssessmentCount(_selectedCourseId) == 1 && DatabaseService.GetPerformanceAssessmentCount(_selectedCourseId) == 1)
            {
                await DisplayAlert("Max Assessments Reached", "You cannot add any other assessments.", "OK");
                return;
            }
            if (DatabaseService.GetObjectiveAssessmentCount(_selectedCourseId) == 1 && AssessmentTypePicker.SelectedItem.ToString() == "Objective Assessment")
            {
                await DisplayAlert("Duplicate Objective Assessment", "Please pick a different type of assessment.", "OK");
                return;
            }
            if (DatabaseService.GetPerformanceAssessmentCount(_selectedCourseId) == 1 && AssessmentTypePicker.SelectedItem.ToString() == "Performance Assessment")
            {
                await DisplayAlert("Duplicate Performance Assessment", "Please pick a different type of assessment.", "OK");
                return;
            }

            await DatabaseService.AddAssessment(_selectedCourseId, AssessmentTitle.Text, AssessmentTypePicker.SelectedItem.ToString(),
                DueDatePicker.Date, Notification.IsToggled);
            await Navigation.PopAsync();
        }

        async void CancelAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}