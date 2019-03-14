using System;
using System.Collections.Generic;

namespace BookApp.API.Models {
  public class BookCatalog {
    public int Id { get; set; }
    public int BookId { get; set; }
    public int CatalogId { get; set; }
    public Book Book { get; set; }
    public Catalog Catalog { get; set; }
  }
}