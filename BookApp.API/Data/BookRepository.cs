using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApp.API.Dtos;
using BookApp.API.Helpers;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Data {
  public class BookRepository : IBookRepository {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IAuthorRepository _repoAuthor;

    public BookRepository (DataContext context, IMapper mapper, IAuthorRepository repoAuthor) {
      _context = context;
      _mapper = mapper;
      _repoAuthor = repoAuthor;
    }

    public async Task<Book> AddBook (BookCreateDto bookDto) {

      var result = _mapper.Map<Book> (bookDto);
      result.AddedOn = DateTime.Now;

      if (await _repoAuthor.CheckAuthorExists (bookDto.AuthorName)) {
        var authorResult = await _repoAuthor.Get (bookDto.AuthorName);
        result.AuthorId = authorResult.Id;
      } else {
        var authorResult = await _repoAuthor.Create (bookDto.AuthorName);
        result.AuthorId = authorResult.Id;
      }

      await _context.Books.AddAsync (result);
      await _context.SaveChangesAsync ();

      return result;
    }

    public async Task<BookListActions> AddBookAction (BookActionDto bookActionDto) {
      var result = _mapper.Map<BookListActions> (bookActionDto);
      result.AddedOn = DateTime.Now;

      await _context.BookListActions.AddAsync (result);
      await _context.SaveChangesAsync ();

      return result;
    }

    public async Task<BookCatalog> AddBookToCatalog (BookCatalogCreateDto bookCatalogDto) {

      var bookCatalogEntity = new BookCatalog () {
        BookId = bookCatalogDto.BookId,
        CatalogId = bookCatalogDto.CatalogId.Value, //TODO: Check and create if not exists
        Created = DateTime.Now
      };

      await _context.BookCatalog.AddAsync (bookCatalogEntity);
      await _context.SaveChangesAsync ();

      return bookCatalogEntity;
    }

    public async Task<BookListActions> DeleteBookAction (int bookId = 3) {
      var result = _context.BookListActions.Find (bookId);

      _context.BookListActions.Attach (result);
      _context.BookListActions.Remove (result);
      await _context.SaveChangesAsync ();

      return result;
    }

    public async Task<List<BookPreviewDto>> GetAll () {
      //var bookList = await _context.Books.Include(itm => itm.BookListActions).OrderByDescending(item => item.AddedOn).ToListAsync();
      var bookList = await _context.Books.OrderByDescending (item => item.AddedOn).ToListAsync ();
      var mappedBookList = _mapper.Map<List<BookPreviewDto>> (bookList);

      return mappedBookList;
    }

    public async Task<BookDetailsDto> GetBook (string friendlyUrl) {
     
      var book = await _context.Books.Include(itm=>itm.BookCatalogs).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();
      var mappedBook = _mapper.Map<BookDetailsDto> (book);

      return mappedBook;
    }

  }
}