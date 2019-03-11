using System;

namespace BookApp.API.Dtos
{
  public class AuthorCreateDto
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public string FriendlyUrl { get; set; }

   // public DateTime AddedOn { get; set; }

  }
}