using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F1WebAPI.Models
{
    public class F1Season
    {
        public List<Season> Season { get; set; }
    }

    public class Schedule
    {
        public string Event { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class Season
    {
        public int Year { get; set; }
        public string Country { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string City { get; set; }
        public List<Schedule> Schedule { get; set; }
    }

}