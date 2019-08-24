using System;
using System.Collections.Generic;

namespace BookApp.API.Dtos {
  public class CatalogItemDto {

    public CatalogItemDto () {
      this.Books = new List<BookItemDto> ();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public int UserId { get; set; }
    public string FriendlyUrl { get; set; }
    public string UserFriendlyUrl { get; set; }
    public DateTime AddedOn { get; set; }
    public List<BookItemDto> Books { get; set; }
  }
}