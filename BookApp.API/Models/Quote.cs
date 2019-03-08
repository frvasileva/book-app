using System;

namespace BookApp.API.Models
{
  public class Quote
  {
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTime AddedOn { get; set; }

    public Author Author { get; set; }

  }
}