using System;
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
        public string Team { get; set; }
        public string ImageURL { get; set; }
        public string DriverURL { get; set; }
    }
}