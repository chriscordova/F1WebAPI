using ExtensionMethods;
using F1WebAPI.ActionFilters;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using F1WebAPI.Models;

namespace F1WebAPI.Controllers
{
    [CustomAuthorize]
    [RoutePrefix("api/scrape")]
    public class ScrapeController : ApiController
    {
        [HttpGet()]
        [Route("seasons")]
        public JsonResult<ApiResponse> ScrapeSeasons()
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

            return Json(new ApiResponse() { Success = true });
        }

        [HttpGet()]
        [Route("drivers")]
        public JsonResult<ApiResponse> ScrapeDrivers()
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

            string controllerURL4 = Functions.GetConfigValue("driverBioURL");
            if (!controllerURL4.IsNullOrEmpty())
            {
                string[] drivers = Functions.GetConfigValue("CurrentDrivers").Split(',').ToArray();
                if (drivers.Length > 0)
                {
                    drivers.ToList().ForEach(driver =>
                    {
                        string controllerURL5 = controllerURL4.Replace("$name$", Functions.CleanName( driver.Replace(" ","-").ToLower()));
                        string html = Functions.GetHTMLFromURL(controllerURL5);
                        if (!html.IsNullOrEmpty())
                        {
                            string s = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\driver-bio-" + driver + "-scrape.html");

                            using (StreamWriter sw = new StreamWriter(s))
                            {
                                sw.Write(html);
                            }
                        }
                    });
                }
            }

            return Json(new ApiResponse() { Success = true });
        }

        [HttpGet()]
        [Route("standings")]
        public JsonResult<ApiResponse> ScrapeStandings()
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

            return Json(new ApiResponse() { Success = true });
        }

        [HttpGet()]
        [Route("results")]
        public JsonResult<ApiResponse> ScrapeResults()
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

            return Json(new ApiResponse() { Success = true });
        }
    }
}
