using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
