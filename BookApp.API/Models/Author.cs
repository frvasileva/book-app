using System;
using System.Collections.Generic;

namespace BookApp.API.Models {
  public class Author {
    public Author () {
      var idGen = new IdGeneratorHelper ();
      this.Id = idGen.Generate ();
      this.AddedOn = DateTime.Now;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FriendlyUrl { get; set; }
    public DateTime AddedOn { get; set; }
    public ICollection<Book> Books { get; set; }

  }
}