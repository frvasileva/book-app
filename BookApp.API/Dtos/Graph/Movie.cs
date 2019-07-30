using Newtonsoft.Json;

namespace Neo.Models {
  public class Movie {
    [JsonProperty (PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty (PropertyName = "released")]
    public int Released { get; set; }

    [JsonProperty (PropertyName = "tagline")]
    public string TagLine { get; set; }
  }
}