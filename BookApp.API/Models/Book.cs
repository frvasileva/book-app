using System.Collections.Generic;
using System;

namespace BookApp.API.Models
{
  public class Book
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PhotoPath { get; set; }
    public string FriendlyUrl { get; set; }

    public DateTime AddedOn { get; set; }

    public int? PublisherId { get; set; }

    public Publisher Publisher { get; set; }

    public User User { get; set; }

    public int UserId { get; set; }

    public Author Author { get; set; }
    public int? AuthorId { get; set; }

    public List<BookListActions> BookListActions { get; set; }
  }
}