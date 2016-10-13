using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace F1WebAPI.Models
{
    public class ScrapeHistory
    {
        public class Log
        {
            public int id { get; set; }
            public string scrapename { get; set; }
            public string lastupdated { get; set; }
        }
    }
}