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

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/seasons")]
    public class SeasonsController : ApiController
    {
        List<F1Season> seasons = new List<F1Season>();

        public static List<F1Season> GetSeasonsData(int year, string country)
        {
            List<F1Season> returnData = new List<F1Season>();

            List<Season> allSeasons = new List<Season>();

            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\seasons-" + year + "-" + country +"-scrape.html");
            if (html.IsNullOrEmpty()) return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            List<Schedule> schedules = new List<Schedule>();

            Season season = new Season();
            string date = doc.DocumentNode.SelectNodes("//h2[contains(@class, 'race-data-header')]").FirstOrDefault().InnerText.Clean();
            string[] dateArray = date.Split('–');
            season.DateFrom = dateArray[0];
            season.DateTo = dateArray[1].Substring(0,5);
            season.Year = year;
            season.Schedule = schedules;

            Country racecountry = new Country();
            racecountry.Name = country;

            var circuitNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'circuit-info-container')]").ToList();

            racecountry.FirstGrandPrix = Convert.ToInt32(circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "First Grand Prix")
                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText);
            racecountry.NumberOfLaps = Convert.ToInt32(circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Number\nof Laps")
                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText);
            racecountry.CircuitLength = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Circuit Length")
                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.Clean();
            racecountry.RaceDistance = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Race Distance")
                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.Clean();
            racecountry.LapRecord = circuitNodes.Find(x => x.SelectNodes(".//h5[contains(@class, 'circuit-info-title')]").FirstOrDefault().InnerText == "Lap\nRecord")
                .SelectNodes(".//p[contains(@class, 'circuit-info-value')]").FirstOrDefault().InnerText.Clean();

            //string scheduleNodes = doc.DocumentNode.SelectNodes("//dl[contains(@class, 'race-data-dl')]//").FirstOrDefault().InnerText;
            season.RaceCountry = racecountry;

            allSeasons.Add(season);

            F1Season f1season = new F1Season();
            f1season.Season = allSeasons;

            returnData.Add(f1season);
            /////////////////////////////////

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

        [Route("{year:int}/{country}")]
        public IHttpActionResult GetSeasonByCountry(int year, string country)
        {
            List<F1Season> myStandings = GetSeasonsData(year, country);

            Season mySeason = new Season();
            List<Season> mySeasons = new List<Season>();
            foreach (F1Season item in myStandings)
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
