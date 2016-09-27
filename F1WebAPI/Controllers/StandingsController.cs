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
        List<F1Standings> Standings = new List<F1Standings>();

        public static List<F1Standings> GetDriverStandingsData(string year)
        {
            List<F1Standings> returnData = new List<F1Standings>();
            List<DriverStanding> allStandings = new List<DriverStanding>();
            F1Standings f1Standings = new F1Standings();

            string controllerURL = "/en/results.html/" + year + "/drivers.html";
            string html = Functions.GetHTMLFromURL(controllerURL);
            if (html.IsNullOrEmpty()) return null;
            
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

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

            f1Standings.Standings = allStandings;
            returnData.Add(f1Standings);

            return returnData;
        }

        [Route("{year}")]
        public IEnumerable<F1Standings> GetAllStandings(string year)
        {
            return GetDriverStandingsData(year);
        }

        [Route("{year}/{position}")]
        public IHttpActionResult GetDriverByYearAndPosition(string year, int position)
        {
            List<F1Standings> myStandings = GetDriverStandingsData(year);
            DriverStanding myDriverStanding = new DriverStanding();
            foreach (F1Standings item in myStandings)
            {
                var a = item.Standings;
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
    }
}
