using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971.Models;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C971.Services;
using System.Text.RegularExpressions;

namespace C971.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCourse : ContentPage
    {
        private readonly int _selectedTermId;

        public AddCourse()
        {
            InitializeComponent();
        }

        public AddCourse(int termId)
        {
            InitializeComponent();
            _selectedTermId = termId;
        }

        private bool emailIsValid()
        {
            try
            {
                MailAddress email = new MailAddress(CourseInstructorEmail.Text);
                return true;
            }
            catch(FormatException) {
                return false;
            }
        }

        private bool phoneIsvalid()
        {
            try{
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
            catch (Exception){
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
            if (phoneIsvalid() == false) {
                await DisplayAlert("Invalid Phone number", "Please enter a valid phone number", "OK");
                return;
            }
            if (DatabaseService.CompareDates(StartDatePicker.Date, EndDatePicker.Date) == false)
            {
                await DisplayAlert("Invalid Start Date", "Start Date Must be Before End Date!", "OK");
                return;
            }

            await DatabaseService.AddCourse(_selectedTermId, CourseTitle.Text, StartDatePicker.Date, EndDatePicker.Date,
                CourseStatusPicker.SelectedItem.ToString(),
                CourseInstructorName.Text, CourseInstructorPhone.Text, CourseInstructorEmail.Text,
                Notification.IsToggled, Notification2.IsToggled, NotesEditor.Text); ;
            await Navigation.PopAsync();
        }

        async void CancelCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Home_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}