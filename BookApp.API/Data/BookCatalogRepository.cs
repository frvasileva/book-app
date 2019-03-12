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
  public class BookCatalogRepository : IBookCatalogRepository
  {

    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookCatalogRepository(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<BookCatalog> Create(BookListCreateDto bookListDto)
    {
      var result = _mapper.Map<BookCatalog>(bookListDto);
      result.Created = DateTime.Now;
      
      await _context.BookCatalog.AddAsync(result);
      await _context.SaveChangesAsync();

      return result;
    }

    public async Task<BookCatalog> Delete(int id)
    {
      var result = _context.BookCatalog.Find(id);

      _context.BookCatalog.Attach(result);
      _context.BookCatalog.Remove(result);
      await _context.SaveChangesAsync();

      return result;
    }

    public async Task<BookListItemDto> Get(int id)
    {
      var bookList = await _context.BookCatalog.Where(item => item.Id == id).FirstOrDefaultAsync();
      var mappedBook = _mapper.Map<BookListItemDto>(bookList);

      return mappedBook;
    }

    public async Task<List<BookListItemDto>> GetAll()
    {
      var bookList = await _context.BookCatalog.OrderByDescending(item => item.Created).ToListAsync();
      var mappedBookList = _mapper.Map<List<BookListItemDto>>(bookList);

      return mappedBookList;
    }

    public Task<BookCatalog> Update()
    {
      throw new System.NotImplementedException();
    }
  }
}