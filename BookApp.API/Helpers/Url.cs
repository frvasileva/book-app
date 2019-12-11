using System.Collections.Generic;

namespace BookApp.API.Helpers {

  public static class Url {
    public static string GenerateFriendlyUrl (string url) => CleanNonUrlSymbols (url);

    private static string CleanNonUrlSymbols (string url) {
      Transliteration translate = new Transliteration ();

      var translatedUrl = translate.BulgarianToEnglish (StringHelper.CollapseSpaces (url).ToLower ());

      var forbiddenUrlSymbols = new Dictionary<string, string> () { { ".", "" }, { ",", "" }, { ";", "" }, { ":", "" }, { "{", "" }, { "}", "" }, { "(", "" }, { ")", "" }, { "^", "" }, { "<", "" }, { ">", "" }, { "`", "" }, { "!", "" }, { "?", "" }, { "&", "-" }, { "/", "-" }, { @"""", "" }, { "»", "" }, { "~", "" }
        };

      foreach (KeyValuePair<string, string> pair in forbiddenUrlSymbols) {
        translatedUrl = translatedUrl.Replace (pair.Key, pair.Value);
      }

      return translatedUrl;
    }
  }
}