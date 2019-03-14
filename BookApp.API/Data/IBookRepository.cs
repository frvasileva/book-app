using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Data
{
  public interface IBookRepository
  {
    Task<List<BookPreviewDto>> GetAll();
    Task<BookDetailsDto> GetBook(string friendlyUrl);
    Task<Book> AddBook(BookCreateDto book);
    Task<BookListActions> AddBookAction(BookActionDto book);

    Task<BookListActions> DeleteBookAction(int bookId);
    Task<BookCatalog> AddBookToCatalog(BookCatalogCreateDto bookCatalogDto);

  }
}