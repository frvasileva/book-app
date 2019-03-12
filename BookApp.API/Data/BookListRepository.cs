using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data
{
  public class BookListRepository : IBookListRepository
  {

    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookListRepository(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<BookList> Create(BookListCreateDto bookListDto)
    {
      var result = _mapper.Map<BookList>(bookListDto);
      result.Created = DateTime.Now;
      
      await _context.BookList.AddAsync(result);
      await _context.SaveChangesAsync();

      return result;
    }

    public async Task<BookList> Delete(int id)
    {
      var result = _context.BookList.Find(id);

      _context.BookList.Attach(result);
      _context.BookList.Remove(result);
      await _context.SaveChangesAsync();

      return result;
    }

    public async Task<BookListItemDto> Get(int id)
    {
      var bookList = await _context.BookList.Where(item => item.Id == id).FirstOrDefaultAsync();
      var mappedBook = _mapper.Map<BookListItemDto>(bookList);

      return mappedBook;
    }

    public async Task<List<BookListItemDto>> GetAll()
    {
      var bookList = await _context.BookList.OrderByDescending(item => item.Created).ToListAsync();
      var mappedBookList = _mapper.Map<List<BookListItemDto>>(bookList);

      return mappedBookList;
    }

    public Task<BookList> Update()
    {
      throw new System.NotImplementedException();
    }
  }
}