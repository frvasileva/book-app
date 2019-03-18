using System.Collections.Generic;
using BookApp.API.Models;

namespace BookApp.API.Dtos {
  public class BookDetailsDto {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string PhotoPath { get; set; }

    public string FriendlyUrl { get; set; }

    public ICollection<BookCatalog> BookCatalogs { get; set; }

  }
}