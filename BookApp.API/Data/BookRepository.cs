using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data
{
  public class BookRepository : IBookRepository
  {
    private readonly DataContext _context;
    public BookRepository(DataContext context)
    {
      _context = context;
    }

    public async Task<List<BookPreviewDto>> GetAll()
    {
      var bookList = await _context.Books.ToListAsync();
      var bookListDto = new List<BookPreviewDto>();

      foreach (var item in bookList)
      {
        var itm = new BookPreviewDto
        {
          Id = item.Id,
          Title = item.Title,
          Description = item.Description
        };

        bookListDto.Add(itm);
      }

      return bookListDto;
    }

    public async Task<BookDetailsDto> GetBook(string friendlyUrl)
    {
      // TODO - add friendly url and get the specific book!
      var book = await _context.Books.FirstOrDefaultAsync();

      var bookDto = new BookDetailsDto
      {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description,
        PhotoPath = book.PhotoPath
      };

      return bookDto;
    }
  }
}