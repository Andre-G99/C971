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
    public partial class EditAssessment : ContentPage
    {
        private readonly int _selectedAssessmentID;
        public EditAssessment()
        {
            InitializeComponent();
        }

        public EditAssessment(Assessment selectedAssessment)
        {
            InitializeComponent();
            _selectedAssessmentID = selectedAssessment.ID;
            AssessmentTitle.Text = selectedAssessment.Title;
            DueDatePicker.Date = selectedAssessment.DueDate;
            if (DatabaseService.GetObjectiveAssessmentCount(selectedAssessment.CourseID) == 1
                && DatabaseService.GetPerformanceAssessmentCount(selectedAssessment.CourseID) == 1)
            {
                AssessmentTypePicker.IsEnabled = false;
            }
            else {
                AssessmentTypePicker.IsEnabled = true;
            }

            AssessmentTypePicker.SelectedItem = selectedAssessment.Type;
            Notification.IsToggled = selectedAssessment.PushNotification;
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

            await DatabaseService.UpdateAssessment(_selectedAssessmentID, AssessmentTitle.Text, AssessmentTypePicker.SelectedItem.ToString(),
                DueDatePicker.Date, Notification.IsToggled);
            await Navigation.PopAsync();
        }

        async void CancelAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void DeleteAssessment_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete this Assessment?", "Delete this Assessment?", "Yes", "No");

            if (answer == true)
            {
                var id = _selectedAssessmentID;
                await DatabaseService.RemoveAssessment(id);
                await DisplayAlert("Assessment Deleted", "Assessment Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Delete Canceled", "Assessment was not deleted", "OK");
            }

            await Navigation.PopAsync();
        }
    }
}