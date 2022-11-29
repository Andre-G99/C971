using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SQLite;
using Xamarin.Essentials;
using C971.Models;

namespace C971.Services
{
    public static class DatabaseService
    {

        #region Term methods

        public static async Task AddTerm(string title, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term()
            {
                Title = title,
                StartDate = start,
                EndDate = end
            };

            await _db.InsertAsync(term);

            var id = term.ID; // Returns the term id
        }
        public static async Task RemoveTerm(int id)
        {
            await Init();

            await _db.DeleteAsync<Term>(id);
        }
        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var terms = await _db.Table<Term>().ToListAsync();
            return terms;
        }
        public static async Task UpdateTerm(int id, string title, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await _db.Table<Term>().Where(i => i.ID == id).FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Title = title;
                termQuery.StartDate = start;
                termQuery.EndDate = end;

                await _db.UpdateAsync(termQuery);
            }
        }

        #endregion

        #region Course methods
        public static async Task AddCourse(int termID, string title, DateTime start, DateTime end, string status, string name, string phone, string email, bool pushStart,
            bool pushEnd, string notes)
        {
            await Init();

            var course = new Course
            {
                TermID = termID,
                Title = title,
                StartDate = start,
                EndDate = end,
                Status = status,
                InstructorName = name,
                InstructorPhone = phone,
                InstructorEmail = email,
                pushNotificationStart = pushStart,
                pushNotificationEnd = pushEnd,
                Notes = notes
            };

            await _db.InsertAsync(course);

            var id = course.ID; //Returns course id
        }
        public static async Task RemoveCourse(int id)
        {
            await Init();

            await _db.DeleteAsync<Course>(id);
        }
        public static async Task<IEnumerable<Course>> GetCourses(int termID)
        {
            await Init();

            var courses = await _db.Table<Course>().Where(i => i.TermID == termID).ToListAsync();

            return courses;
        }
        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();
            var courses = await _db.Table<Course>().ToListAsync();

            return courses;
        }
        public static async Task UpdateCourse(int id, string title, DateTime start, DateTime end, string status, string name, string phone,
            string email, bool pushStart, bool pushEnd, string notes)
        {
            await Init();

            var courseQuery = await _db.Table<Course>().Where(i => i.ID == id).FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Title = title;
                courseQuery.StartDate = start;
                courseQuery.EndDate = end;
                courseQuery.Status = status;
                courseQuery.InstructorName = name;
                courseQuery.InstructorPhone = phone;
                courseQuery.InstructorEmail = email;
                courseQuery.pushNotificationStart = pushStart;
                courseQuery.pushNotificationEnd = pushEnd;
                courseQuery.Notes = notes;

                await _db.UpdateAsync(courseQuery);
            }
        }

        #endregion

        #region Assessment methods

        public static async Task AddAssessment(int courseID, string title, string type, DateTime dueDate, bool pushNotif)
        {
            await Init();
            var assessment = new Assessment()
            {
                CourseID = courseID,
                Title = title,
                Type = type,
                DueDate = dueDate,
                PushNotification = pushNotif
            };

            await _db.InsertAsync(assessment);

            var id = assessment.ID; // Returns the term id
        }
        public static async Task RemoveAssessment(int id)
        {
            await Init();

            await _db.DeleteAsync<Assessment>(id);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments(int courseID)
        {
            await Init();

            var assessments = await _db.Table<Assessment>().Where(i => i.CourseID == courseID).ToListAsync();

            return assessments;
        }
        public static int GetPerformanceAssessmentCount(int courseID)
        {

            int count = _db.Table<Assessment>().Where(i => i.CourseID == courseID && i.Type == "Performance Assessment").ToListAsync().Result.Count;

            return count;
        }
        public static int GetObjectiveAssessmentCount(int courseID)
        {

            int count = _db.Table<Assessment>().Where(i => i.CourseID == courseID && i.Type == "Objective Assessment").ToListAsync().Result.Count;

            return count;
        }
        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();

            var assessmentList = await _db.Table<Assessment>().ToListAsync();
            return assessmentList;
        }
        public static async Task UpdateAssessment(int id, string title, string type, DateTime dueDate, bool pushNotif)
        {
            await Init();

            var assessmentQuery = await _db.Table<Assessment>().Where(i => i.ID == id).FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Title = title;
                assessmentQuery.DueDate = dueDate;
                assessmentQuery.Type = type;
                assessmentQuery.PushNotification = pushNotif;

                await _db.UpdateAsync(assessmentQuery);
            }
        }

        #endregion

        #region Demo Data
        public static async void InsertSampleData()
        {
            await Init();

            Term term = new Term
            {
                Title = "Term 1",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.AddDays(15).Date,
            };

            _DB.Insert(term);

            Course course = new Course
            {
                TermID = term.ID,
                Title = "Course 1",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.AddDays(15).Date,
                Status = "In Progress",
                InstructorName = "Andre Garibay",
                InstructorPhone = "818-721-1942",
                InstructorEmail = "agari11@wgu.edu",
                pushNotificationStart = true,
                pushNotificationEnd = false,
                Notes = "Here are some notes."
            };

            _DB.Insert(course);

            Assessment assessment = new Assessment
            {
                CourseID = course.ID,
                Title = "C971 PA",
                Type = "Performance Assessment",
                DueDate = DateTime.Now.AddDays(5).Date,
                PushNotification = true,
            };

            _DB.Insert(assessment);

            Assessment assessment2 = new Assessment
            {
                CourseID = course.ID,
                Title = "C971 OA",
                Type = "Objective Assessment",
                DueDate = DateTime.Now.AddDays(5).Date,
                PushNotification = false,
            };

            _DB.Insert(assessment2);

        }
        public static async Task ClearSampleData()
        {
            await Init();

            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Assessment>();

            _db = null;
            _dbConnection = null;
        }
        public static async Task<int> GetWidgetCountAsync(int selectedGadgetId)
        {
            //TODO getting a widget count from a table.
            int widgetCount = await _db.ExecuteScalarAsync<int>($"Select Count(*) from Widget where GadgetID = ?", selectedGadgetId);

            return widgetCount;
        }
        #endregion

        private static SQLiteAsyncConnection _db;
        private static SQLiteConnection _DB;
        private static SQLiteConnection _dbConnection;

        public static bool CompareDates(DateTime start, DateTime end)
        {
            if (start < end) { return true; } //returns true if start date is valid
            else { return false; } //else returns false
        }

        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }
            //Get an absolute path to the database file
            var databasePath = Path.Combine
                    (FileSystem.AppDataDirectory, "Terms.db");

            _db = new SQLiteAsyncConnection(databasePath);
            _DB = new SQLiteConnection(databasePath);
            _dbConnection = new SQLiteConnection(databasePath);

            _DB.CreateTable<Term>();
            _DB.CreateTable<Course>();
            _DB.CreateTable<Assessment>();

            //asynchronously adding the tables would cause a big delay in them showing up on the first run

            //await _db.CreateTableAsync<Term>();
            //await _db.CreateTableAsync<Course>();
            //await _db.CreateTableAsync<Assessment>();
        }
    }
}
