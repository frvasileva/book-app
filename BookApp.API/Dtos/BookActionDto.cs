using System;
using BookApp.API.Models;

namespace BookApp.API.Dtos
{
  public class BookActionDto
  {
    public int Id { get; set; }
    public BookActionTypeEnum BookListType { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }
  }
}