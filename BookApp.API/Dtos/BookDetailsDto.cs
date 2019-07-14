using System.Collections.Generic;
using BookApp.API.Models;

namespace BookApp.API.Dtos {
  public class BookDetailsDto {

    public BookDetailsDto () {
      this.BookCatalogs = new List<BookCatalogListDto> ();
    }
    
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string PhotoPath { get; set; }

    public string FriendlyUrl { get; set; }

    public List<BookCatalogListDto> BookCatalogs { get; set; }

  }
}