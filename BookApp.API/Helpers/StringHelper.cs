using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BookApp.API.Helpers
{
    public static class StringHelper
    {
        public static string CollapseSpaces(string value)
        {
            return Regex.Replace(value, @"\s+", " "); ;
        }

        public static string HtmlStrip(string htmlString)
        {
            var stripedHtml = Regex.Replace(htmlString, @"<[^>]*>", "");

            return stripedHtml;
        }

        public static string ReplaceQuoteVariantsFromString(string value)
        {
            var quoteVariantsList = new Dictionary<string, string>() {
            { "\"","" },
            { "“","" },
            { "„","" },
            { "'","" },
            { "”","" } };

            foreach (KeyValuePair<string, string> pair in quoteVariantsList)
            {
                value = value.Replace(pair.Key, pair.Value);
            }

            var forbiddenUrlSymbols = new Dictionary<string, string>() {
                { ".","" },
                { ",","" },
                { ";","" },
                { ":","" },
                { "{","" },
                { "}","" },
                { "(","" },
                { ")","" },
                { "^","" },
                { "<","" },
                { ">","" },
                { "`","" },
                { "!","" },
                { "?","" },
                { "&","-" },
                { "/","-" },
                { @"""","" },
                { "»","" },
                { "~","" }
            };

            foreach (KeyValuePair<string, string> pair in forbiddenUrlSymbols)
            {
                value = value.Replace(pair.Key, pair.Value);
            }

            return value;
        }
    }
}