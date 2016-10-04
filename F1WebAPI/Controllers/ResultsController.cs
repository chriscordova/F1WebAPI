using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using F1WebAPI.Models;
using ExtensionMethods;
using HtmlAgilityPack;

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/results")]
    public class ResultsController : ApiController
    {
        public static List<F1Results> GetResults(int year, string country)
        {
            List<F1Results> returnData = new List<F1Results>();

            string html = Functions.GetHTMLFromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\results-" + year + "-" + country + "-scrape.html");
            if (html.IsNullOrEmpty()) return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.CleanHTML(false));

            List<Result> results = new List<Result>();
            var resultTableNodes = doc.DocumentNode.SelectNodes("//table[contains(@class, 'resultsarchive-table')]//tr").ToList();

            int iCount = 0;
            foreach (HtmlNode n in resultTableNodes)
            {
                if (iCount == 0)
                {
                    iCount++;
                    continue;
                }

                Result raceResult = new Result();
                raceResult.Position = n.SelectNodes(".//td[2]").FirstOrDefault().InnerText;
                raceResult.Number = Convert.ToInt32(n.SelectNodes(".//td[3]").FirstOrDefault().InnerText);
                raceResult.Driver = n.SelectNodes(".//td[4]//span[1]").FirstOrDefault().InnerText + " " + n.SelectNodes(".//td[4]//span[2]").FirstOrDefault().InnerText;
                raceResult.Team = n.SelectNodes(".//td[5]").FirstOrDefault().InnerText;
                raceResult.Laps = Convert.ToInt32(n.SelectNodes(".//td[6]").FirstOrDefault().InnerText);
                raceResult.TimeOrRetiredResult = n.SelectNodes(".//td[7]").FirstOrDefault().InnerText;
                raceResult.Points = Convert.ToInt32(n.SelectNodes(".//td[8]").FirstOrDefault().InnerText);

                results.Add(raceResult);
            }

            F1Results f1results = new F1Results();
            f1results.Results = results;

            returnData.Add(f1results);

            return returnData;
        }

        [Route("{year:int}/{country}")]
        public IHttpActionResult GetResultByYearAndCountry(int year, string country)
        {
            List<F1Results> f1results = GetResults(year, country);
            return Ok(f1results);
        }
    }
}
