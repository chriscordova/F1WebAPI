using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using F1WebAPI.Models;
using ExtensionMethods;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.IO;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Web.Script.Serialization;

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/seasons")]
    public class SeasonsController : ApiController
    {
        List<F1Season> seasons = GetSeasonsData();

        public static List<OrderedSeason> GetSeasonCountryOrdered(string year)
        {
            List<OrderedSeason> aReturn = new List<OrderedSeason>();
            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\seasons-" + year + "-scrape.html");
            if (html.IsNullOrEmpty()) return aReturn;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.CleanHTML(false));

            var aSections = doc.DocumentNode.SelectNodes("//div[contains(@class, 'article-columns')]//div").ToList();
            foreach (var section in aSections)
            {
                bool bRaced = false;
                string href = section.SelectNodes(".//a").FirstOrDefault().Attributes.Where(a => a.Name == "href").ToList().FirstOrDefault().Value;
                var raceWinnerNode = section.SelectNodes(".//p[contains(@class, 'race-winner')]");
                if (!raceWinnerNode.IsNull<HtmlNodeCollection>()) bRaced = true;

                string sYearURL = Functions.GetConfigValue("seasonsURL").Replace("$year$", year).Replace(".html", "");
                string sCountry = href.Replace(sYearURL + "/", "").Replace(".html","");
                if (!sCountry.Contains("webcal:"))
                {
                    OrderedSeason item = new OrderedSeason();
                    item.CountryName = sCountry;
                    item.Complete = bRaced;
                    item.Year = Convert.ToInt32( year );

                    aReturn.Add(item);
                }
            }

            return aReturn;
        }

        public static List<F1Season> GetSeasonsData()
        {
            List<F1Season> returnData = new List<F1Season>();

            List<Season> allSeasons = new List<Season>();

            string[] years = Functions.GetConfigValue("yearsArray").Split(',').ToArray();
            if (years.Length > 0)
            {
                years.ToList().ForEach(year =>
                {
                    //string[] countries = Functions.GetConfigValue("countryArray").Split(',').ToArray();
                    var countries = GetSeasonCountryOrdered(year).ToArray();
                    if (countries.Length > 0)
                    {
                        countries.ToList().ForEach(country =>
                        {
                            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\seasons-" + year + "-" + country.CountryName + "-scrape.html");
                            if (html.IsNullOrEmpty()) return; 

                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(html.CleanHTML(false));

                            List<Schedule> schedules = new List<Schedule>();

                            Season season = new Season();
                            string date = doc.DocumentNode.SelectNodes("//h2[contains(@class, 'race-data-header')]").FirstOrDefault().InnerText;
                            string[] dateArray = date.Split('–');
                            season.DateFrom = dateArray[0].Trim();
                            season.DateTo = dateArray[1].Trim().Substring(0, 6);
                            season.Month = dateArray[1].Trim().Substring(7, 3);
                            season.Year = Convert.ToInt32(year);
                            season.Complete = country.Complete;
                            
                            var scheduleNodes = doc.DocumentNode.SelectNodes("//dl[contains(@class, 'race-data-dl')]").FirstOrDefault();
                            
                            var eventNodes = scheduleNodes.SelectNodes(".//dt[contains(@class, 'race-data-item')]").ToList();
                            var dateNodes = scheduleNodes.SelectNodes(".//dd[contains(@class, 'day')]").ToList();
                            var timeNodes = scheduleNodes.SelectNodes(".//dd[contains(@class, 'time')]").ToList();

                            for (int i = 0; i < 5; i++)
                            {
                                Schedule schedule = new Schedule();
                                schedule.Event = eventNodes.ElementAt(i).InnerText;
                                schedule.Date = dateNodes.ElementAt(i).InnerText;
                                schedule.Time = timeNodes.ElementAt(i).InnerText;

                                schedules.Add(schedule);
                            }
                            
                            season.Schedule = schedules;

                            Country racecountry = new Country();
                            racecountry.Name = country.CountryName;

                            var circuitNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'circuit-info-container')]").ToList();

                            racecountry.FirstGrandPrix = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "First Grand Prix")
                                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.CleanString();
                            racecountry.NumberOfLaps = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Numberof Laps")
                                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.CleanString();
                            racecountry.CircuitLength = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Circuit Length")
                                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.CleanString();
                            racecountry.RaceDistance = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Race Distance")
                                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.CleanString();
                            racecountry.LapRecord = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "LapRecord")
                                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.CleanString();
                            racecountry.CircuitImageURL = Functions.GetConfigValue("BaseURL") + doc.DocumentNode.SelectNodes("//div[contains(@class, 'track-map')]").FirstOrDefault().SelectNodes(".//img").FirstOrDefault().Attributes[0].Value;

                            season.RaceCountry = racecountry;

                            allSeasons.Add(season);
                        });
                    }
                });
            }

            F1Season f1season = new F1Season();
            f1season.Season = allSeasons;

            returnData.Add(f1season);

            return returnData;
        }

        [Route("")]
        public IEnumerable<F1Season> GetAllSeasons()
        {
            return seasons;
        }

        [Route("{year:int}")]
        public IHttpActionResult GetSeasonByYear(int year)
        {
            List<Season> mySeasons = new List<Season>();
            foreach (F1Season item in seasons)
            {
                var a = item.Season;
                a.ForEach(s =>
                {
                    if (s.Year == year) mySeasons.Add(s);
                });
                
            }

            if (mySeasons.IsNull())
            {
                return NotFound();
            }

            return Ok(mySeasons);
        }

        [Route("{country}")]
        public IHttpActionResult GetSeasonByCountry(string country)
        {
            List<Season> mySeasons = new List<Season>();
            foreach (F1Season item in seasons)
            {
                var a = item.Season;
                a.ForEach(s =>
                {
                    if (s.RaceCountry.Name == country) mySeasons.Add(s);
                });

            }

            if (mySeasons.IsNull())
            {
                return NotFound();
            }

            return Ok(mySeasons);
        }

        [Route("{year:int}/{country}")]
        public IHttpActionResult GetSeasonByCountry(int year, string country)
        {
            Season mySeason = new Season();
            List<Season> mySeasons = new List<Season>();
            foreach (F1Season item in seasons)
            {
                var a = item.Season;
                a.ForEach(s =>
                {
                    if (s.Year == year) mySeasons.Add(s);
                });

            }

            if (!mySeasons.IsNull())
            {
                foreach (Season item in mySeasons)
                {
                    if (item.RaceCountry.Name.ToLower() == country.ToLower()) mySeason = item;
                }
            }

            if (mySeason.IsNull())
            {
                return NotFound();
            }

            return Ok(mySeason);
        }
    }
}
