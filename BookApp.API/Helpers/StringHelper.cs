using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BookApp.API.Helpers {
    public static class StringHelper {
        public static string CollapseSpaces (string value) {
            return Regex.Replace (value, @"\s+", " ");;
        }

        public static string HtmlStrip (string htmlString) {
            var stripedHtml = Regex.Replace (htmlString, @"<[^>]*>", "");

            return stripedHtml;
        }

        public static string ReplaceQuoteVariantsFromString (string value) {
            var quoteVariantsList = new Dictionary<string, string> () { { "\"", "" }, { "“", "" }, { "„", "" }, { "'", "" }, { "”", "" }
                };

            foreach (KeyValuePair<string, string> pair in quoteVariantsList) {
                value = value.Replace (pair.Key, pair.Value);
            }

            var forbiddenUrlSymbols = new Dictionary<string, string> () { { ".", "" }, { ",", "" }, { ";", "" }, { ":", "" }, { "{", "" }, { "}", "" }, { "(", "" }, { ")", "" }, { "^", "" }, { "<", "" }, { ">", "" }, { "`", "" }, { "!", "" }, { "?", "" }, { "&", "-" }, { "/", "-" }, { @"""", "" }, { "»", "" }, { "~", "" }
                };

            foreach (KeyValuePair<string, string> pair in forbiddenUrlSymbols) {
                value = value.Replace (pair.Key, pair.Value);
            }

            return value;
        }

        public static string GenerateRandomNo () {
            var random = new Random ();
            return random.Next (0, 9999).ToString ("D4");
        }

        public static string UppercaseFirst (string s) {
            // Check for empty string.
            if (string.IsNullOrEmpty (s)) {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper (s[0]) + s.Substring (1);
        }
    }
}