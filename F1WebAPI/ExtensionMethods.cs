using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace ExtensionMethods
{
    public static class MyMethodHelpers
    {
        public static bool IsNullOrEmpty(this String str)
        {
            return str == null || str == "";
        }

        public static bool IsNullOrEmpty(this String[] strArray)
        {
            return strArray == null || strArray.Length == 0;
        }

        public static bool IsNull<T>(this T obj)
        {
            return obj == null;
        }

        

        public static void Clean(this String str)
        {
            Functions.ReplaceWhitespace(str.Replace("\r", "").Replace("\n", ""), "");
        }

    }

    public static class Functions
    {
        private static readonly Regex regexWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return regexWhitespace.Replace(input, replacement);
        }

        public static DateTime FormatDate()
        {
            DateTime date = new DateTime();

            return date;
        }

        public static string GetHTMLFromURL(string controllerURL)
        {
            string sReturn = string.Empty;
            string baseURL = WebConfigurationManager.AppSettings["BaseURL"];
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseURL + controllerURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream d = response.GetResponseStream())
            {
                using (StreamReader r = new StreamReader(d))
                {
                    sReturn = r.ReadToEnd();
                }
            }

            return sReturn;
        }


    }
}