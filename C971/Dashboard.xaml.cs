using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971.Services;
using C971.Models;
using C971.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            TermCollectionView.ItemsSource = await DatabaseService.GetTerms();

            var courseList = await DatabaseService.GetCourses();
            var assessmentList = await DatabaseService.GetAssessments();
            var notifyRandom = new Random();
            var notifyId = notifyRandom.Next(1000);

            foreach (Course record in courseList)
            {
                if (record.pushNotificationStart && record.StartDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Notice", $"{record.Title} starts today " + DateTime.Now.Date.ToString("MM/dd/yyyy") + "!", notifyId);
                }
                if (record.pushNotificationEnd && record.EndDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Notice", $"{record.Title} ends today " + DateTime.Now.Date.ToString("MM/dd/yyyy") + "!", notifyId);
                }
            }

            foreach (Assessment record in assessmentList)
            {
                if (record.PushNotification && record.DueDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Notice", $"{record.Title} is due today " + DateTime.Now.Date.ToString("MM/dd/yyyy") + "!", notifyId);
                }
            }
        }

        async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }

        async void TermCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new EditTerm(term));
            }
        }

        async void AddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }
    }
}