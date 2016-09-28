using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ExtensionMethods;

namespace F1WebAPI.ActionFilters
{
    public class CustomAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private bool IsValid(Dictionary<string, string> Credentials)
        {
            if (Credentials["token"] == Functions.GetConfigValue("scrapeToken"))
                return true;
            else
                return false;
        }

        private Dictionary<string, string> ParseRequestHeaders(HttpActionContext actionContext)
        {
            Dictionary<string, string> credentials = new Dictionary<string, string>();
            var httpRequestHeader = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();

            string httpRequestHeaderValue = httpRequestHeader;
            credentials.Add("token", httpRequestHeaderValue);

            return credentials;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.ToArray().Length == 0)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    Dictionary<string, string> credentials = ParseRequestHeaders(actionContext);
                    if (!IsValid(credentials))
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }   
}