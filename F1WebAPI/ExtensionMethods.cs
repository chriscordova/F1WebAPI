using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        public static string CleanHTML(this String str, bool removeWhitespace = true)
        {
            if (removeWhitespace)
            {
                return WebUtility.HtmlDecode(Functions.ReplaceWhitespace(str.Replace("\r", "").Replace("\n", ""), ""));
            }
            else
            {
                return WebUtility.HtmlDecode(str.Replace("\r", "").Replace("\n", ""));
            }
        }

        public static string CleanString(this String str)
        {
            return str.Replace("\t", "").Trim();
        }

    }

    public static class Functions
    {
        private static readonly Regex regexWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return regexWhitespace.Replace(input, replacement);
        }

        public static string CleanName(string s)
        {
            var decomposed = s.Normalize(NormalizationForm.FormD);
            var filtered = decomposed.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            var newString = new String(filtered.ToArray());

            return newString;
        }

        public static DateTime FormatDate(string sDate)
        {
            DateTime date = new DateTime();
            date = DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            return date;
        }

        public static int GetAge(DateTime birthDate)
        {
            int age = 0;
            var today = DateTime.Today;
            age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            return age;
        }

        public static string GetHTMLFromURL(string controllerURL)
        {
            string sReturn = string.Empty;
            string baseURL = GetConfigValue("baseURL");

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

        internal static string GetScrapedTeamName(string TeamName)
        {
            string sReturn = string.Empty;
            switch (TeamName.Trim())
            {
                case "Red Bull Racing":
                    {
                        sReturn = "Red-Bull";
                    }
                    break;
                case "MRT":
                    {
                        sReturn = "Manor";
                    }
                    break;
                case "Force India":
                    {
                        sReturn = "Force-India";
                    }
                    break;
                case "Toro Rosso":
                    {
                        sReturn = "Toro-Rosso";
                    }
                    break;
                default:
                    {
                        sReturn = TeamName.Trim();
                    }
                    break;
            }

            return sReturn;
        }

        public static string GetHTMLFromFile(string fileURL)
        {
            return File.ReadAllText(fileURL);
        }

        public static string GetConfigValue(string key)
        {
            string Return = string.Empty;
            string value = WebConfigurationManager.AppSettings[key];
            if (!value.IsNullOrEmpty()) Return = value;

            return Return;
        }


    }
}