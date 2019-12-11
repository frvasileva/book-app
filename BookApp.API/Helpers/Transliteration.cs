using System.Collections.Generic;

namespace BookApp.API.Helpers
{
    public class Transliteration
    {
        Dictionary<string, string> cyrilicToEnglish = new Dictionary<string, string>() {
            { "а","a" },
            { "б","b" },
            { "в","v" },
            { "г","g" },
            { "д","d" },
            { "е","e" },
            { "ж","zh" },
            { "з","z" },
            { "и","i" },
            { "й","i" },
            { "к","k" },
            { "л","l" },
            { "м","m" },
            { "н","n" },
            { "о","o" },
            { "п","p" },
            { "р","r" },
            { "с","s" },
            { "т","t" },
            { "у","u" },
            { "ф","f" },
            { "х","h" },
            { "ц","tz" },
            { "ч","ch" },
            { "ш","sh" },
            { "щ","sht" },
            { "ъ","a" },
            { "ь","j" },
            { "ю","ju" },
            { "я","ja" },
            { " ","-" },
            //{ "","" },
            //{ "","" },
        };

        /// <summary>
        /// Return transliterated string from Bulgarian to English
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string BulgarianToEnglish(string word)
        {

            foreach (KeyValuePair<string, string> pair in cyrilicToEnglish)
            {
                word = word.Replace(pair.Key, pair.Value);
            }

            return word;
        }

        /// <summary>
        /// Return transliterated string from English to Bulgarian
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string EnglishToBulgarian(string word)
        {
            foreach (KeyValuePair<string, string> pair in cyrilicToEnglish)
            {
                word = word.Replace(pair.Value, pair.Key);
            }
            return word;
        }
    }
}
