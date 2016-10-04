using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScrapySharp.Network;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using F1WebAPI.Models;
using System.IO;
using ExtensionMethods;
using System.Web.Configuration;

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/drivers")]
    public class DriversController : ApiController
    {
        List<F1Drivers> Drivers = GetDriversData();

        public static List<F1Drivers> GetDriversData()
        {
            List<F1Drivers> returnData = new List<F1Drivers>();
            List<Driver> allDrivers = new List<Driver>();
            F1Drivers f1Drivers = new F1Drivers();

            string baseURL = WebConfigurationManager.AppSettings["BaseURL"];
            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\drivers-scrape.html");
            if (html.IsNullOrEmpty()) return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.CleanHTML(false));

            var DriverNodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'driver-teaser')]").ToList();

            foreach (HtmlNode n in DriverNodes)
            {
                Driver driver = new Driver();
                driver.Name = n.SelectNodes(".//h1[contains(@class, 'driver-name')]").FirstOrDefault().InnerText;
                driver.DriverURL = baseURL + n.SelectNodes("//a[contains(@class, 'driver-title')]").FirstOrDefault().Attributes[0].Value;
                driver.DriverNumber = Convert.ToInt32(n.SelectNodes(".//div[contains(@class, 'driver-number')]").FirstOrDefault().InnerText);
                driver.ImageURL = baseURL + n.SelectNodes(".//img[contains(@class, 'fom-image')]").FirstOrDefault().Attributes.Where(s => s.Name == "src").FirstOrDefault().Value;

                Team team = new Team();
                string teamNameNode = n.SelectNodes(".//p[contains(@class, 'driver-team')]").FirstOrDefault().InnerText.Trim();

                //get team url and fill Team class here
                string teamHTML = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\team-"+ Functions.GetScrapedTeamName(teamNameNode) + "-scrape.html");

                HtmlDocument teamDoc = new HtmlDocument();
                teamDoc.LoadHtml(teamHTML.CleanHTML(false));

                var teamNodes = teamDoc.DocumentNode.SelectNodes("//table[contains(@class, 'stat-list')]").ToList();

                foreach(HtmlNode t in teamNodes)
                {
                    team.Name = teamNameNode;
                    team.FullName = t.SelectNodes(".//tr[1]//td").FirstOrDefault().InnerText;
                    team.Base = t.SelectNodes(".//tr[2]//td").FirstOrDefault().InnerText;
                    team.TeamChief = t.SelectNodes(".//tr[3]//td").FirstOrDefault().InnerText;
                    team.TechnicalChief = t.SelectNodes(".//tr[4]//td").FirstOrDefault().InnerText;
                    team.Chassis = t.SelectNodes(".//tr[5]//td").FirstOrDefault().InnerText;
                    team.PowerUnit = t.SelectNodes(".//tr[6]//td").FirstOrDefault().InnerText;
                    team.FirstTeamEntry = t.SelectNodes(".//tr[7]//td").FirstOrDefault().InnerText;
                    team.WorldChampionships = t.SelectNodes(".//tr[8]//td").FirstOrDefault().InnerText;
                    team.HighestRaceFinish = t.SelectNodes(".//tr[9]//td").FirstOrDefault().InnerText;
                    team.PolePositions = t.SelectNodes(".//tr[10]//td").FirstOrDefault().InnerText;
                    team.FastestLaps = t.SelectNodes(".//tr[11]//td").FirstOrDefault().InnerText;
                }

                driver.Team = team; 

                allDrivers.Add(driver);
            }

            f1Drivers.Drivers = allDrivers;
            returnData.Add(f1Drivers);

            return returnData;
        }

        [Route("")]
        public IEnumerable<F1Drivers> GetAllDrivers()
        {
            return Drivers;
        }

        [Route("{number:int}")]
        public IHttpActionResult GetDriverByNumber(int number)
        {
            List<Driver> myDrivers = new List<Driver>();
            foreach (F1Drivers item in Drivers)
            {
                var a = item.Drivers;
                a.ForEach(s =>
                {
                    if (s.DriverNumber == number) myDrivers.Add(s);
                });

            }

            if (myDrivers.IsNull())
            {
                return NotFound();
            }

            return Ok(myDrivers);
        }
    }
}
