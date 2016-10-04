using ExtensionMethods;
using F1WebAPI.ActionFilters;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Filters;

namespace F1WebAPI.Controllers
{
    [CustomAuthorize]
    [RoutePrefix("api/scrape")]
    public class ScrapeController : ApiController
    {
        [HttpGet()]
        [Route("seasons")]
        public IHttpActionResult ScrapeSeasons()
        {
            string[] years = Functions.GetConfigValue("yearsArray").Split(',').ToArray();
            if (years.Length > 0)
            {
                years.ToList().ForEach(y =>
                {
                    string[] countries = Functions.GetConfigValue("countryArray").Split(',').ToArray();
                    if (countries.Length > 0)
                    {
                        countries.ToList().ForEach(c =>
                        {
                            string controllerURL = Functions.GetConfigValue("seasonsCountryURL").Replace("$year$", y).Replace("$country$", c);
                            if (!controllerURL.IsNullOrEmpty())
                            {
                                string html = Functions.GetHTMLFromURL(controllerURL);
                                if (!html.IsNullOrEmpty())
                                {
                                    string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\seasons-" + y + "-" + c + "-scrape.html");
                                    using (StreamWriter sw = new StreamWriter(s))
                                    {
                                        sw.Write(html);
                                    }
                                }
                            }
                        });
                    }
                });
            }

            return Ok();
        }

        [HttpGet()]
        [Route("drivers")]
        public IHttpActionResult ScrapeDrivers()
        {
            string controllerURL = Functions.GetConfigValue("driversURL");
            if (!controllerURL.IsNullOrEmpty())
            {
                string html = Functions.GetHTMLFromURL(controllerURL);
                if (!html.IsNullOrEmpty())
                {
                    string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\drivers-scrape.html");

                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(html);
                    }
                }
            }

            string controllerURL2 = Functions.GetConfigValue("teamsURL");
            if (!controllerURL2.IsNullOrEmpty())
            {
                string[] teams = Functions.GetConfigValue("teamsArray").Split(',').ToArray();
                if (teams.Length > 0)
                {
                    teams.ToList().ForEach(team =>
                    {
                        string controllerURL3 = controllerURL2.Replace("$team$", team);
                        string html2 = Functions.GetHTMLFromURL(controllerURL3);
                        if (!html2.IsNullOrEmpty())
                        {
                            string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\team-"+ team +"-scrape.html");

                            using (StreamWriter sw = new StreamWriter(s))
                            {
                                sw.Write(html2);
                            }
                        }
                    });
                }       
            }

            return Ok();
        }

        [HttpGet()]
        [Route("standings")]
        public IHttpActionResult ScrapeStandings()
        {
            string[] years = Functions.GetConfigValue("yearsArray").Split(',').ToArray();
            if (years.Length > 0)
            {
                years.ToList().ForEach(y =>
                {
                    string controllerURL = Functions.GetConfigValue("driverStandingsURL").Replace("$year$", y);
                    string html = Functions.GetHTMLFromURL(controllerURL);
                    if (!html.IsNullOrEmpty())
                    {
                        string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\driverstandings-" + y + "-scrape.html");
                        using (StreamWriter sw = new StreamWriter(s))
                        {
                            sw.Write(html);
                        }
                    }
                    
                    string controllerURL2 = Functions.GetConfigValue("constructorStandingsURL").Replace("$year$", y);
                    string html2 = Functions.GetHTMLFromURL(controllerURL2);
                    if (!html2.IsNullOrEmpty())
                    {
                        string s2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\constructorstandings-" + y + "-scrape.html");
                        using (StreamWriter sw = new StreamWriter(s2))
                        {
                            sw.Write(html2);
                        }
                    }
                    
                });
            }

            return Ok();
        }

        [HttpGet()]
        [Route("results")]
        public IHttpActionResult ScrapeResults()
        {
            string[] years = Functions.GetConfigValue("yearsArray").Split(',').ToArray();
            if (years.Length > 0)
            {
                years.ToList().ForEach(y =>
                {
                    string[] countries = Functions.GetConfigValue(y + "Results").Split(',').ToArray();
                    if (countries.Length > 0)
                    {
                        countries.ToList().ForEach(c =>
                        {
                            string controllerURL = Functions.GetConfigValue("resultsURL").Replace("$year$", y).Replace("$country$", c);
                            if (!controllerURL.IsNullOrEmpty())
                            {
                                string html = Functions.GetHTMLFromURL(controllerURL);
                                if (!html.IsNullOrEmpty())
                                {
                                    string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\results-" + y + "-" + c.Substring(c.IndexOf('/') + 1) + "-scrape.html");
                                    using (StreamWriter sw = new StreamWriter(s))
                                    {
                                        sw.Write(html);
                                    }
                                }
                            }
                        });
                    }
                });
            }

            return Ok();
        }
    }
}
