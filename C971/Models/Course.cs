using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int TermID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public bool pushNotificationStart { get; set; }
        public bool pushNotificationEnd { get; set; }
        public string Notes { get; set; }
    }
}
