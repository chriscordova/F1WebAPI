﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F1WebAPI.Models
{
    public class F1Drivers
    {
        public List<Driver> Drivers { get; set; }
    }

    public class Driver
    {
        public int DriverNumber { get; set; }
        public string Name { get; set; }
        public Team Team { get; set; }
        public string ImageURL { get; set; }
        public string DriverURL { get; set; }
        public Biography Biography { get; set; }
    }

    public class Biography
    {
        public string Country { get; set; }
        public string Podiums { get; set; }
        public int GrandPrixsEntered { get; set; }
        public string Points { get; set; }
        public string WorldChampionships { get; set; }
        public string HighestRaceFinish { get; set; }
        public string HighestGridPosition { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string PlaceOfBirth { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Base { get; set; }
        public string TeamChief { get; set; }
        public string TechnicalChief { get; set; }
        public string Chassis { get; set; }
        public string PowerUnit { get; set; }
        public string FirstTeamEntry { get; set; }
        public string WorldChampionships { get; set; }
        public string HighestRaceFinish { get; set; }
        public string PolePositions { get; set; }
        public string FastestLaps { get; set; }
    }
}