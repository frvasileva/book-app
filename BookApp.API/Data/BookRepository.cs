using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data
{
  public class BookRepository : IBookRepository
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookRepository(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<Book> AddBook(BookCreateDto bookDto)
    {
      var result = _mapper.Map<Book>(bookDto);

      result.UserId = 2;
      await _context.Books.AddAsync(result);
      await _context.SaveChangesAsync();

      return result;
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