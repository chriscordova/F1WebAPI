using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using F1WebAPI.Models;
using NanoApi;
using F1WebAPI.ActionFilters;
using ExtensionMethods;

namespace F1WebAPI.Controllers
{
    [RoutePrefix("api/scrapelog")]
    public class ScrapeHistoryController : ApiController
    {
        [HttpGet()]
        [Route("get/{type}")]
        public ApiResponse GetScrapeLastUpdated(string type)
        {
            string lastUpdated = string.Empty;
            try
            {
                var db = NanoApi.JsonFile<ScrapeHistory.Log>.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\", "scrape_history.json");
                string driversLastUpdated = db.Select(s => s.scrapename == type).FirstOrDefault().lastupdated;
                if (!driversLastUpdated.IsNullOrEmpty()) lastUpdated = driversLastUpdated;
            }
            catch (Exception ex)
            {
                return new ApiResponse() { Success = false, ErrorMessage = ex.Message };
            }
            

            return new ApiResponse() { Success = true, Data = lastUpdated };
        }

        [CustomAuthorize]
        [HttpPost()]
        [Route("update/{type}")]
        public ApiResponse UpdateScrapeLastUpdated(string type)
        {
            try
            {
                var db = NanoApi.JsonFile<ScrapeHistory.Log>.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\", "scrape_history.json");
                db.Update(p => p.scrapename == type, p => p.lastupdated = DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                return new ApiResponse() { Success = false, ErrorMessage = ex.Message };
            }
            

            return new ApiResponse() { Success = true, Data = DateTime.Now.ToString() };
        }


    }
}
