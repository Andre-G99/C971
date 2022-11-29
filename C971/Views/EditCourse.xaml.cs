using C971.Models;
using C971.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCourse : ContentPage
    {
        private readonly int _selectedCourseID;

        public EditCourse()
        {
            InitializeComponent();
        }

        public EditCourse(Course selectedCourse)
        {
            InitializeComponent();
            _selectedCourseID = selectedCourse.ID;
            CourseTitle.Text = selectedCourse.Title;
            StartDatePicker.Date = selectedCourse.StartDate;
            EndDatePicker.Date = selectedCourse.EndDate;
            CourseStatusPicker.SelectedItem = selectedCourse.Status;
            CourseInstructorName.Text = selectedCourse.InstructorName;
            CourseInstructorPhone.Text = selectedCourse.InstructorPhone;
            CourseInstructorEmail.Text = selectedCourse.InstructorEmail;
            NotesEditor.Text = selectedCourse.Notes;
            Notification.IsToggled = selectedCourse.pushNotificationStart;
            Notification2.IsToggled = selectedCourse.pushNotificationEnd;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            AssessmentCollectionView.ItemsSource = await DatabaseService.GetAssessments(_selectedCourseID);
        }

        async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete this Course?", "Delete this Course?", "Yes", "No");

            if (answer == true)
            {
                var id = _selectedCourseID;
                await DatabaseService.RemoveCourse(id);
                await DisplayAlert("Course Deleted", "Course Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Delete Canceled", "Course was not deleted", "OK");
            }

            await Navigation.PopAsync();
        }
        private bool emailIsValid()
        {
            try
            {
                MailAddress email = new MailAddress(CourseInstructorEmail.Text);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool phoneIsvalid()
        {
            try
            {
                Regex reg = new Regex(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$");

                if (reg.IsMatch(CourseInstructorPhone.Text))
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CourseTitle.Text))
            {
                await DisplayAlert("Missing Title", "Please enter a title.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CourseInstructorName.Text))
            {
                await DisplayAlert("Missing Instructor Name", "Please enter a name.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CourseStatusPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Color", "Please enter a status.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CourseInstructorPhone.Text))
            {
                await DisplayAlert("Missing Instructor Phone Number", "Please enter a phone number.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CourseInstructorEmail.Text))
            {
                await DisplayAlert("Missing Instructor Email", "Please enter an email.", "OK");
                return;
            }
            if (emailIsValid() == false)
            {
                await DisplayAlert("Invalid Email Address", "Please enter a valid email address.", "OK");
                return;
            }
            if (phoneIsvalid() == false)
            {
                await DisplayAlert("Invalid Phone number", "Please enter a valid phone number", "OK");
                return;
            }
            if (DatabaseService.CompareDates(StartDatePicker.Date, EndDatePicker.Date) == false)
            {
                await DisplayAlert("Invalid Start Date", "Start Date Must be Before End Date!", "OK");
                return;
            }

            await DatabaseService.UpdateCourse(_selectedCourseID, CourseTitle.Text, StartDatePicker.Date, EndDatePicker.Date,
                CourseStatusPicker.SelectedItem.ToString(),
                CourseInstructorName.Text, CourseInstructorPhone.Text, CourseInstructorEmail.Text,
                Notification.IsToggled, Notification2.IsToggled, NotesEditor.Text); ;
            await Navigation.PopAsync();
        }

        async void CancelCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void ShareButton_Clicked(object sender, EventArgs e)
        {
            var text = NotesEditor.Text;
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Text"
            });
        }

        async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            var courseId = _selectedCourseID;

            await Navigation.PushAsync(new AddAssessment(_selectedCourseID));
        }

        async void AssessmentCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var assessment = (Assessment)e.CurrentSelection.FirstOrDefault();
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new EditAssessment(assessment));
            }
        }
    }
}