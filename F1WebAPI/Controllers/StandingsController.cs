using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using F1WebAPI.Models;
using HtmlAgilityPack;
using ExtensionMethods;
using System.IO;

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/standings")]
    public class StandingsController : ApiController
    {
        #region ##########################################Driver Standings################################################
        List<F1DriverStandings> Standings = new List<F1DriverStandings>();

        public static List<F1DriverStandings> GetDriverStandingsData(string year)
        {
            List<F1DriverStandings> returnData = new List<F1DriverStandings>();
            List<DriverStanding> allStandings = new List<DriverStanding>();
            F1DriverStandings f1Standings = new F1DriverStandings();

            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\driverstandings-" + year + "-scrape.html");
            if (html.IsNullOrEmpty()) return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.CleanHTML(false));

            var DriverStandingNodes = doc.DocumentNode.SelectNodes("//table[contains(@class, 'resultsarchive-table')]//tr").ToList();

            int iCount = 0;
            foreach (HtmlNode n in DriverStandingNodes)
            {
                if (iCount == 0)
                {
                    iCount++;
                    continue;
                }

                DriverStanding standing = new DriverStanding();

                standing.Position = iCount;
                standing.Driver = n.SelectNodes(".//td[3]/a/span[1]").FirstOrDefault().InnerText + " " + n.SelectNodes(".//td[3]/a/span[2]").FirstOrDefault().InnerText;
                standing.Nationality = n.SelectNodes(".//td[4]").FirstOrDefault().InnerText;
                standing.Team = n.SelectNodes(".//td[5]/a").FirstOrDefault().InnerText;
                standing.Points = Convert.ToInt32(n.SelectNodes(".//td[6]").FirstOrDefault().InnerText);


                allStandings.Add(standing);
                iCount++;
            }

            f1Standings.DriverStandings = allStandings;
            returnData.Add(f1Standings);

            return returnData;
        }

        [Route("drivers/{year}")]
        public IEnumerable<F1DriverStandings> GetAllDriverStandings(string year)
        {
            return GetDriverStandingsData(year);
        }

        [Route("drivers/{year}/{position}")]
        public IHttpActionResult GetDriverByYearAndPosition(string year, int position)
        {
            List<F1DriverStandings> myStandings = GetDriverStandingsData(year);
            DriverStanding myDriverStanding = new DriverStanding();
            foreach (F1DriverStandings item in myStandings)
            {
                var a = item.DriverStandings;
                a.ForEach(s =>
                {
                    if (s.Position == position) myDriverStanding = s;
                });

            }

            if (myDriverStanding.IsNull())
            {
                return NotFound();
            }

            return Ok(myDriverStanding);
        }
        #endregion

        #region #########################################Constructor Standings############################################
        public static List<F1ConstructorStandings> GetConstructorStandingsData(string year)
        {
            List<F1ConstructorStandings> returnData = new List<F1ConstructorStandings>();
            List<ConstructorStanding> allStandings = new List<ConstructorStanding>();
            F1ConstructorStandings f1Standings = new F1ConstructorStandings();

            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\constructorstandings-" + year + "-scrape.html");
            if (html.IsNullOrEmpty()) return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var ConstructorStandingNodes = doc.DocumentNode.SelectNodes("//table[contains(@class, 'resultsarchive-table')]//tr").ToList();

            int iCount = 0;
            foreach (HtmlNode n in ConstructorStandingNodes)
            {
                if (iCount == 0)
                {
                    iCount++;
                    continue;
                }

                ConstructorStanding standing = new ConstructorStanding();

                standing.Position = iCount;
                standing.Team = n.SelectNodes(".//td[3]/a").FirstOrDefault().InnerText;
                standing.Points = Convert.ToInt32(n.SelectNodes(".//td[4]").FirstOrDefault().InnerText);


                allStandings.Add(standing);
                iCount++;
            }

            f1Standings.ConstructorStandings = allStandings;
            returnData.Add(f1Standings);

            return returnData;
        }

        [Route("constructor/{year}")]
        public IEnumerable<F1ConstructorStandings> GetAllConstructorStandings(string year)
        {
            return GetConstructorStandingsData(year);
        }

        [Route("constructor/{year}/{position}")]
        public IHttpActionResult GetConstructorByYearAndPosition(string year, int position)
        {
            List<F1ConstructorStandings> myStandings = GetConstructorStandingsData(year);
            ConstructorStanding myConstructorStanding = new ConstructorStanding();
            foreach (F1ConstructorStandings item in myStandings)
            {
                var a = item.ConstructorStandings;
                a.ForEach(s =>
                {
                    if (s.Position == position) myConstructorStanding = s;
                });

            }

            if (myConstructorStanding.IsNull())
            {
                return NotFound();
            }

            return Ok(myConstructorStanding);
        }
        #endregion
    }
}
