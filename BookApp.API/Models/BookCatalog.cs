using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApp.API.Models {
  public class BookCatalog {

    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int BookId { get; set; }
    public int CatalogId { get; set; }
    public Book Book { get; set; }
    public Catalog Catalog { get; set; }
    public DateTime Created { get; set; }

  }
}