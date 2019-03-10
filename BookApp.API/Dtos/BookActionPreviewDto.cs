using BookApp.API.Models;

namespace BookApp.API.Dtos
{
  public class BookActionPreviewDto
  {
    public int Id { get; set; }

    public BookActionTypeEnum BookListType { get; set; }

  }
}