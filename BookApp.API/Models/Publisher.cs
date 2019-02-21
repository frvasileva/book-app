using System.Collections.Generic;

namespace BookApp.API.Models
{
  public class Publisher
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Book> Books { get; set; }

  }
}