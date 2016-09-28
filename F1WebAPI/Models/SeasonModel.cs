﻿using Newtonsoft.Json;
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
        public Country RaceCountry { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<Schedule> Schedule { get; set; }
    }

    public class Country
    {
        public string Name { get; set; }
        public int FirstGrandPrix { get; set; }
        public int NumberOfLaps { get; set; }
        public string CircuitLength { get; set; }
        public string RaceDistance { get; set; }
        public string LapRecord { get; set; }
        public string CircuitImageURL { get; set; }

    }

}