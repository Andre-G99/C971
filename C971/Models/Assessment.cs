using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set;}
        public bool PushNotification { get; set; }
    }
}
