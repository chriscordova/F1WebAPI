using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F1WebAPI.Models
{
    public class F1Standings
    {
        public List<DriverStanding> Standings { get; set; }
    }

    public class DriverStanding
    {
        public int Position { get; set; }
        public string Driver { get; set; }
        public string Nationality { get; set; }
        public string Team { get; set; }
        public int Points { get; set; }
    }
}