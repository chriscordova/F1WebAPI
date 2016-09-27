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

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/seasons")]
    public class SeasonsController : ApiController
    {
        List<F1Season> seasons = GetSeasonsData();

        public static List<F1Season> GetSeasonsData()
        {
            List<F1Season> returnData = new List<F1Season>();

            using (var r = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Resources/db.json"))
            {
                string json = r.ReadToEnd();
                returnData = JsonConvert.DeserializeObject<List<F1Season>>(json);
            }
           
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
                    if (item.Country.ToLower() == country.ToLower()) mySeason = item;
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
