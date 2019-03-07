using System.Collections.Generic;
using System.Linq;
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
      var mappedBookList = _mapper.Map<List<BookPreviewDto>>(bookList);

      return mappedBookList;
    }

    public async Task<BookDetailsDto> GetBook(string friendlyUrl)
    {
      var book = await _context.Books.Where(item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync();
      var mappedBook = _mapper.Map<BookDetailsDto>(book);

      return mappedBook;
    }


  }
}