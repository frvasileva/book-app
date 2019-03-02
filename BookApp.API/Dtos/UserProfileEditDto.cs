using System.ComponentModel.DataAnnotations;

namespace BookApp.API.Dtos
{
  public class UserProfileEditDto
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string AvatarPath { get; set; }
    public string KnownAs { get; set; }
    public string Introduction { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
  }
}
