using System.Collections.Generic;
using System.Threading.Tasks;
using BookApp.API.Dtos;
using BookApp.API.Models;

namespace BookApp.API.Data {
  public interface IBookGraphRepository {
    Task<List<BookPreviewDto>> GetAll ();
    Task<BookDetailsDto> GetBook (string friendlyUrl);
    Task<List<BookPreviewDto>> GetBooksAddedByUser (string friendlyUrl);
    BookCreateDto AddBook (BookCreateDto book);

  }
}