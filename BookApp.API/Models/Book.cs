using System.Collections.Generic;

namespace BookApp.API.Models
{
  public class Book
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PhotoPath { get; set; }
    public ICollection<Publisher> MessagesSent { get; set; }
  }
}