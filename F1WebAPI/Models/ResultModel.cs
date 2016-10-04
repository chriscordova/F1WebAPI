using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F1WebAPI.Models
{
    public class F1Results
    {
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public string Position { get; set; }
        public int Number { get; set; }
        public string Driver { get; set; }
        public string Team { get; set; }
        public int Laps { get; set; }
        public string TimeOrRetiredResult { get; set; }
        public int Points { get; set; }
    }
}