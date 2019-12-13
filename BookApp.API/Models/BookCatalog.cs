using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApp.API.Models {
  public class BookCatalog {

    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int BookId { get; set; }
    public int CatalogId { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }
  }
}