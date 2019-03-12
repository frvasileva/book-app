using System;
using System.Collections.Generic;

namespace BookApp.API.Models
{
  public class BookCatalog
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPublic { get; set; }
    public DateTime Created { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public List<Book> Books { get; set; }
  }
}