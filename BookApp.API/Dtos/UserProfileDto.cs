using System;
using System.Collections.Generic;
using BookApp.API.Models;

namespace BookApp.API.Dtos {
  public class UserProfileDto {

    public int Id { get; set; }
    public string Email { get; set; }
    public string AvatarPath { get; set; }
    public string UserName { get; set; }
    public string KnownAs { get; set; }
    public string Introduction { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string FriendlyUrl { get; set; }
    public bool IsFollowedByCurrentUser { get; set; }

    public List<BookDetailsDto> Books { get; set; }
  }
}
