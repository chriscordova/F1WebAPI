using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/scrape")]
    public class ScrapeController : ApiController
    {
        [Route("seasons")]
        public IHttpActionResult ScrapeSeasons()
        {

            return Ok();
        }

        [HttpGet()]
        [Route("drivers")]
        public IHttpActionResult ScrapeDrivers()
        {
            string baseURL = WebConfigurationManager.AppSettings["BaseURL"];
            string controllerURL = "/en/championship/drivers.html";
            string html = Functions.GetHTMLFromURL(controllerURL);
            string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\drivers-scrape.html");

            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.Write(html);
            }

            return Ok();
        }
        
        [HttpGet()]
        [Route("standings")]
        public IHttpActionResult ScrapeStandings()
        {
            string[] years = new string[] { "2015", "2016" };
            string baseURL = WebConfigurationManager.AppSettings["BaseURL"];

            years.ToList().ForEach(y =>
            {
                string controllerURL = "/en/results.html/" + y + "/drivers.html";
                string html = Functions.GetHTMLFromURL(controllerURL);
                string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\standings-" + y + "-scrape.html");

                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(html);
                }
            });
            

            return Ok();
        }
    }
}
