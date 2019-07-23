using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Data {
  public interface IBookRepository {
    Task<List<BookPreviewDto>> GetAll ();
    Task<BookDetailsDto> GetBook (string friendlyUrl);
    Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl);
    Task<Book> AddBook (BookCreateDto book);
    Task<BookListActions> AddBookAction (BookActionDto book);

    Task<bool> RemoveBookFromCatalog (int catalogId, int bookId);
    Task<BookCatalog> AddBookToCatalog (BookCatalogCreateDto bookCatalogDto);

    void Add<T> (T entity) where T : class;
    void Delete<T> (T entity) where T : class;
    Task<bool> SaveAll ();
    Task<Book> Get (string friendlyUrl);

    Task<Book> ImportBooks ();
    Task<Tag> ImportTags ();
    Task<BookTags> ImportBookTags ();
  }
}