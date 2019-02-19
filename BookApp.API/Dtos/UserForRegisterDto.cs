using System.ComponentModel.DataAnnotations;

namespace BookApp.API.Dtos
{
  public class UserForRegisterDto
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }


    [Required]
    [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
    public string Password { get; set; }
  }
}