using System.ComponentModel.DataAnnotations;

namespace BookApp.API.Dtos {
  public class UserProfileEditDto {
    public string AvatarPath { get; set; }
    public string KnownAs { get; set; }
    public string Introduction { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string FriendlyUrl { get; set; }
  }
}