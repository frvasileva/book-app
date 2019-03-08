using System;

namespace BookApp.API.Models
{
  public class BookListActions
  {
    public int Id { get; set; }

    public BookActionTypeEnum BookListType { get; set; }

    public DateTime AddedOn { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }
  }
}