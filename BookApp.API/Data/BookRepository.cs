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

    public void Add<T> (T entity) where T : class {
      throw new NotImplementedException ();
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
        Created = DateTime.Now,
        UserId = bookCatalogDto.UserId
      };

      if (bookCatalogDto.CatalogId.HasValue) {
        bookCatalogEntity.CatalogId = bookCatalogDto.CatalogId.Value;
      } else {
        var catalog = new Catalog ();
        catalog.UserId = bookCatalogDto.UserId;
        catalog.IsPublic = true;
        catalog.Name = bookCatalogDto.CatalogName;
        catalog.FriendlyUrl = Url.GenerateFriendlyUrl (catalog.Name + "-" + StringHelper.GenerateRandomNo ());
        catalog.Created = DateTime.Now;

        _context.Catalogs.Add (catalog);
        await _context.SaveChangesAsync ();

        bookCatalogEntity.CatalogId = catalog.Id;

      }
      await _context.BookCatalog.AddAsync (bookCatalogEntity);
      await _context.SaveChangesAsync ();

      return bookCatalogEntity;
    }

    public void Delete<T> (T entity) where T : class {
      throw new NotImplementedException ();
    }

    public async Task<bool> RemoveBookFromCatalog (int catalogId, int bookId) {
      var result = _context.BookCatalog.Where (item => item.CatalogId == catalogId && item.BookId == bookId).ToList().FirstOrDefault();

      _context.BookCatalog.Attach (result);
      _context.BookCatalog.Remove (result);

      return await _context.SaveChangesAsync () > 0;
    }

    public async Task<Book> Get (string friendlyUrl) {
      var book = await _context.Books.Include (itm => itm.BookCatalogs).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();
      return book;
    }

    public async Task<List<BookPreviewDto>> GetAll () {
      var bookList = await _context.Books.Include (itm => itm.BookCatalogs).OrderByDescending (item => item.AddedOn).ToListAsync ();
      //  var mappedBookList = _mapper.Map<List<BookPreviewDto>> (bookList);
      var mappedBookList = new List<BookPreviewDto> ();

      foreach (var item in bookList) {
        var bookPreview = new BookPreviewDto () {
          Id = item.Id,
          UserId = item.UserId,
          Title = item.Title,
          Description = item.Description,
          PhotoPath = item.PhotoPath,
          FriendlyUrl = item.FriendlyUrl
        };

        foreach (var itm in item.BookCatalogs) {
          var bookCatalogListDto = new BookCatalogListDto () {
            CatalogId = itm.CatalogId
          };

          bookPreview.BookCatalogs.Add (bookCatalogListDto);
        }

        mappedBookList.Add (bookPreview);
      }

      return mappedBookList;
    }

    public async Task<BookDetailsDto> GetBook (string friendlyUrl) {

      var book = await _context.Books.Include (itm => itm.BookCatalogs).Where (item => item.FriendlyUrl == friendlyUrl).FirstOrDefaultAsync ();

      var mappedBook = new BookDetailsDto () {
        Id = book.Id,
        UserId = book.UserId,
        Title = book.Title,
        Description = book.Description,
        PhotoPath = book.PhotoPath,
        FriendlyUrl = book.FriendlyUrl
      };

      foreach (var itm in book.BookCatalogs) {
        var bookCatalogListDto = new BookCatalogListDto () {
          CatalogId = itm.CatalogId
        };

        mappedBook.BookCatalogs.Add (bookCatalogListDto);
      }

      return mappedBook;
    }

    public async Task<bool> SaveAll () {
      var res = await _context.SaveChangesAsync ();
      return res > 0;
    }
  }
}